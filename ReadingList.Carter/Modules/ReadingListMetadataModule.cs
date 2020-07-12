using System;
using Carter.OpenApi;
using ReadingList.Logic.Models;

namespace ReadingList.Carter.Modules
{
    public class GetReadingList : RouteMetaData
    {
        public override string Description { get; } = "Returns a list of books from a readinglist";

        public override RouteMetaDataResponse[] Responses { get; } =
        {
            new RouteMetaDataResponse
            {
                Code = 200,
                Description = $"A list of books currently read",
                Response = typeof(Book),
            }
        };
        public override QueryStringParameter[] QueryStringParameter { get; } =
        {
            new QueryStringParameter
            {
                Name = "label",
                Required = false,
                Description = "Filter query by fiction and non-fiction. fact for non-fiction, fiction for fiction",
                Type = typeof(string),
            },
        };

        public override string Tag { get; } = "ReadingList";
    }

    public class GetBacklogList : RouteMetaData
    {
        public override string Description { get; } = "Returns a list of books from a readinglist";

        public override RouteMetaDataResponse[] Responses { get; } =
        {
            new RouteMetaDataResponse
            {
                Code = 200,
                Description = $"A list of books in the backlog",
                Response = typeof(Book),

            }
        };
        public override QueryStringParameter[] QueryStringParameter { get; } =
        {
            new QueryStringParameter
            {
                Name = "label",
                Required = false,
                Description = "Filter query by fiction and non-fiction. fact for non-fiction, fiction for fiction",
                Type = typeof(string),
            }
        };

        public override string Tag { get; } = "ReadingList";
    }

    public class GetDoneList : RouteMetaData
    {
        public override string Description { get; } = "Returns a list of books from a readinglist";

        public override RouteMetaDataResponse[] Responses { get; } =
        {
            new RouteMetaDataResponse
            {
                Code = 200,
                Description = $"A list of books done reading",
                Response = typeof(Book),

            }
        };
        public override QueryStringParameter[] QueryStringParameter { get; } =
        {
            new QueryStringParameter
            {
                Name = "label",
                Required = false,
                Description = "Filter query by fiction and non-fiction. fact for non-fiction, fiction for fiction",
                Type = typeof(string),
            }
        };

        public override string Tag { get; } = "ReadingList";
    }

    public class GetAllList : RouteMetaData
    {
        public override string Description { get; } = "Returns a list of books from all readinglists";

        public override RouteMetaDataResponse[] Responses { get; } =
        {
            new RouteMetaDataResponse
            {
                Code = 200,
                Description = $"Returns a list of books from all readinglists",
                Response = typeof(Book),

            }
        };
        public override QueryStringParameter[] QueryStringParameter { get; } =
        {
            new QueryStringParameter
            {
                Name = "label",
                Required = false,
                Description = "Filter query by fiction and non-fiction. fact for non-fiction, fiction for fiction",
                Type = typeof(string),
            },
        };

        public override string Tag { get; } = "ReadingList";
    }
}
