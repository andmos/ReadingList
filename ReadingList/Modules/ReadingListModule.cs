﻿using System;
using Nancy;
using System.Linq;

namespace ReadingList
{
	public class ReadingListModule : NancyModule 
	{
		private readonly IReadingListService m_readingListService;
		private ITrelloWebHookSources m_webHookSource;
		private ILog m_logger; 

		public ReadingListModule(ITrelloAuthorizationWrapper trelloAuthService, IReadingListService readingListService, ITrelloWebHookSources webHookSource, ILogFactory logger) : base("/api/")
		{
			this.EnableCors();
			m_readingListService = readingListService;
			m_webHookSource = webHookSource;
			m_logger = logger.GetLogger(GetType()); 
			StaticConfiguration.EnableHeadRouting = true; 

			Get["/ping"] = parameters =>
			{
				return "pong";
			};

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

				respons = Response.AsJson("Callback recived");
				respons.StatusCode = HttpStatusCode.OK;
				return respons;
			};
				
		
		
		}
	
	}
}
