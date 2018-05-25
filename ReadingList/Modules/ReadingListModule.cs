﻿using System;
using Nancy;
using System.Linq;
using Nancy.Routing;
using System.Collections.Generic;

namespace ReadingList
{
	public class ReadingListModule : NancyModule
	{
		private readonly IReadingListService m_readingListService;
		private readonly ITrelloWebHookSources m_webHookSource;
		private readonly IReadingBoardService m_readingBoardService;
		private readonly ILog m_logger;

		public ReadingListModule(ITrelloAuthorizationWrapper trelloAuthService, IReadingListService readingListService, IReadingBoardService readingBoardService, ITrelloWebHookSources webHookSource, ILogFactory logger) : base("/api/")
		{
			this.EnableCors();
			m_readingListService = readingListService;
			m_readingBoardService = readingBoardService;
			m_webHookSource = webHookSource;
			m_logger = logger.GetLogger(GetType());
			StaticConfiguration.EnableHeadRouting = true;
			StaticConfiguration.DisableErrorTraces = false;

			Get["/readingList"] = parameters =>
			{
				string requestLabel = Request.Query["label"];
				var readingList = m_readingListService.GetReadingList(TrelloBoardConstans.CurrentlyReading, requestLabel);
				return Response.AsJson(readingList);
			};

			Get["/backlogList"] = parameters =>
			{
				string requestLabel = Request.Query["label"];
				var readingList = m_readingListService.GetReadingList(TrelloBoardConstans.Backlog, requestLabel);
				return Response.AsJson(readingList);
			};

			Get["/doneList"] = parameters =>
			{
				string requestLabel = Request.Query["label"];
				var readingList = m_readingListService.GetReadingList(TrelloBoardConstans.DoneReading, requestLabel);
				return Response.AsJson(readingList);
			};

			Get["/allLists"] = parameters =>
			{
				var allLists = m_readingBoardService.GetAllReadingLists();
				return Response.AsJson(allLists);
			};

			Put["/backlogList"] = parameters =>
			{
				string author = Request.Query["author"];
				string bookTitle = Request.Query["title"];
				string bookLabel = Request.Query["label"];
				Response response;

				if (string.IsNullOrWhiteSpace(author) || string.IsNullOrWhiteSpace(bookTitle) || string.IsNullOrWhiteSpace(bookLabel))
				{
					response = Response.AsJson("author, title and label is required.");
					response.StatusCode = HttpStatusCode.UnprocessableEntity;
					return response;
				}
				try
				{
					if (m_readingListService.AddBookToBacklog(bookTitle, author, bookLabel))
					{
						response = Response.AsJson("created");
						response.StatusCode = HttpStatusCode.Created;

					}
					else
					{
						response = Response.AsJson("Something went wrong when adding book to list");
						response.StatusCode = HttpStatusCode.InternalServerError;
					}

					return response;
				}
				catch(Exception ex)
				{
					response = Response.AsJson(ex.ToString());
					response.StatusCode = HttpStatusCode.InternalServerError;
					return response;
				}
			};

			Put["/doneList"] = parameters =>
			{
				string bookTitle = Request.Query["title"];
				Response response;
				if (string.IsNullOrWhiteSpace(bookTitle))
				{
					response = Response.AsJson("title is needed to move card from reading to done");
					response.StatusCode = HttpStatusCode.UnprocessableEntity;
				}
				try
				{
					m_readingListService.UpdateDoneListFromReadingList(bookTitle);
					response = Response.AsJson("updated");
					response.StatusCode = HttpStatusCode.OK;
					return response;
				}
				catch (Exception ex)
				{
					response = Response.AsJson(ex.ToString());
					response.StatusCode = HttpStatusCode.InternalServerError;
					return response;
				}
			};

			Head["/callBack"] = parameters =>
			{
				Response respons;
				respons = Response.AsJson("Head recived");
				respons.StatusCode = HttpStatusCode.OK;
				return respons;
			};

			Post["/callBack"] = parameters =>
			{
				Response respons;

				if (m_webHookSource.ValidWebhookSources().ToList().Any(source => source.Equals(Request.UserHostAddress)))
				{
					m_logger.Info($"Got Callback from valid source: {Request.UserHostAddress}");
				}

				// TODO: This is a bit leaky..
				if (m_readingListService is ReadingListCache)
				{
					((ReadingListCache)m_readingListService).InvalidateCache();
					m_logger.Info("Invalidating cache");
				}

				respons = Response.AsJson("Callback recived");
				respons.StatusCode = HttpStatusCode.OK;
				return respons;
			};



		}

	}
}
