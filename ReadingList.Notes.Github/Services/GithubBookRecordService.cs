﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Octokit;
using ReadingList.Logging;
using ReadingList.Notes.Github.Helpers;
using ReadingList.Notes.Github.Models;
using Readinglist.Notes.Logic.Models;

namespace ReadingList.Notes.Github.Services
{
    public class GithubBookRecordService : IGithubBookRecordService
    {
        private readonly IBookRecordCache _bookRecordCache;

        private const string UserName = "andmos";
        private const string Repo = "ReadingList-Data";
        private const string NotesFolder = "BookNotes";
        
        private readonly ILog _logger;

        private readonly GithubClient _githubFileClient;

        public GithubBookRecordService(IBookRecordCache bookRecordCache, ILogFactory logFactory, GithubClient githubFileClient)
        {
            _bookRecordCache = bookRecordCache;
            _githubFileClient = githubFileClient;
            _logger = logFactory.GetLogger(GetType());
        }

        public async Task<IEnumerable<BookRecord>> GetAllBookRecords()
        {
            try
            {
                var repo = await _githubFileClient.GetRepositoryContent(UserName, Repo, NotesFolder);

                var bookFiles = new List<BookRecord>();
                foreach (var content in repo)
                {
                    bookFiles.Add(await GetBookRecord(content));
                }

                return bookFiles;
            }
            catch (RateLimitExceededException rateLimitException)
            {
                _logger.Error("Rate limit exceeded against Github, falling back to cache: ", rateLimitException);
                return _bookRecordCache.GetAll().Select(r => r.BookRecord);
            }
        }

        public async Task<BookRecord> GetBookNotes(string book)
        {
            try
            {
                var repo = await _githubFileClient.GetRepositoryContent(UserName, Repo, NotesFolder);
                
                var bookContent = repo?.FirstOrDefault(b => b.Name.ToLower().Contains(book.ToLower()));
                return await GetBookRecord(bookContent);
            }
            catch (RateLimitExceededException rateLimitException)
            {
                _logger.Error("Rate limit exceeded against Github, falling back to cache: ", rateLimitException);
                return _bookRecordCache.GetAll()
                    .FirstOrDefault(r => r.FileName.ToLower().Contains(book.ToLower()))
                    ?.BookRecord;
            }
        }

        private async ValueTask<BookRecord> GetBookRecord(RepositoryContentInfo content)
        {
            if (_bookRecordCache.TryGetValue(content.Sha, out var gitBookRecord))
            {
                return gitBookRecord.BookRecord;
            }
            _logger.Info($"Cache miss for {content.Name} with sha {content.Sha}");
            gitBookRecord = await GetBookRecordFromApi(content);
            _bookRecordCache.TryAdd(content.Sha, gitBookRecord);
            return gitBookRecord.BookRecord;
        }

        private async Task<GitBookRecord> GetBookRecordFromApi(RepositoryContentInfo content)
        {
            var rawBookRecord = await _githubFileClient.GetRepositoryTextFile(content.DownloadUrl);
            var bookRecord = BookRecordParser.CreateBookRecordFromMarkdown(
                new MarkdownFile(rawBookRecord));

            return new GitBookRecord(
                content.Sha, 
                content.Name, 
                bookRecord);
        }
    }
}

