﻿using EducationWebApi.Application.Common;
using EducationWebApi.DataAccess.Common;
using EducationWebApi.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EducationWebApi.DataAccess.Persistence;

public class DatabaseContext : IdentityDbContext<ApplicationUser, AppRole, Guid>, IDatabaseContext
{
    private readonly IMediatorPublisher _mediatorPublisher;
    public DatabaseContext(DbContextOptions<DatabaseContext> options, 
        IMediatorPublisher mediatorPublisher) : base(options)
    {
        _mediatorPublisher = mediatorPublisher;
    }


    public DbSet<Instructor> Instructors  => Set<Instructor>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<StudentInstructor> StudentInstructors => Set<StudentInstructor>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);
    }



    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _mediatorPublisher.DispatchDomainEvents(this);
        return await base.SaveChangesAsync(cancellationToken);
    }




}
