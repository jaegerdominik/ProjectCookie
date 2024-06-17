using ProjectCookie.DAL.BaseClasses;
using ProjectCookie.DAL.BaseInterfaces;
using ProjectCookie.DAL.Entities;
using ProjectCookie.DAL.UnitOfWork;

namespace ProjectCookie.DAL.Repository;

public class ScoreRepository : PostgresRepository<Score>, IScoreRepository
{
    public ScoreRepository(PostgresDbContext context) : base(context) { }
}