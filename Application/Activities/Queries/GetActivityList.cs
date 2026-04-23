using System;
using Domain;
using MediatR;
using Persistence;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using Application.Activities.DTOs;
using Application.Interfaces;
using Application.Core;

namespace Application.Activities.Queries
{
    public class GetActivityList
    {

        public class Query : IRequest<Result<PagedList<ActivityDto, DateTime?>>>
        {
            public required ActivityParams Params { get; set; }
        }
        public class Handler(AppDbContext context, IMapper mapper, IUserAccessor userAccessor) : IRequestHandler<Query, Result<PagedList<ActivityDto, DateTime?>>>
        {
            public async Task<Result<PagedList<ActivityDto, DateTime?>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = context.Activities
                    .OrderBy(a => a.Date)
                    .Where(a => a.Date >= (request.Params.Cursor ?? request.Params.StartDate))
                    .AsQueryable();

                if (!string.IsNullOrEmpty(request.Params.Filter))
                {
                    query = request.Params.Filter switch
                    {
                        "isGoing" => query.Where(a => a.Attendees.Any(u => u.UserId == userAccessor.GetUserId())),
                        "isHost" => query.Where(a => a.Attendees.Any(a=>a.IsHost&&a.UserId==userAccessor.GetUserId())),
                        _ => query
                    };
                }

                var projctedActivities = query.ProjectTo<ActivityDto>(mapper.ConfigurationProvider, new { currentUserId = userAccessor.GetUserId() });

                var activities = await projctedActivities
                    .Take(request.Params.PageSize + 1)
                    .ToListAsync(cancellationToken);

                DateTime? cursor = null;
                if (activities.Count > request.Params.PageSize)
                {
                    cursor= activities.Last().Date;
                    activities.RemoveAt(activities.Count - 1);
                }

                return Result<PagedList<ActivityDto, DateTime?>>.Success(new PagedList<ActivityDto, DateTime?>
                {
                    Items= activities,
                    NextCursor= cursor
                }
                );
            }
        }
    }
}