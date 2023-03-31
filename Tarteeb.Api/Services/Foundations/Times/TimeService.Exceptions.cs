﻿//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Linq;
using EFxceptions.Models.Exceptions;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Tarteeb.Api.Models.Foundations.Times;
using Xeptions;
using Tarteeb.Api.Models.Foundations.Times.Exceptions;

namespace Tarteeb.Api.Services.Foundations.Times
{
    public partial class TimeService
    {
        private delegate IQueryable<Time> ReturningTimesFunction();

        private IQueryable<Time> TryCatch(ReturningTimesFunction returningTimesFunction)
        {
            try
            {
                return returningTimesFunction();
            }
            catch (SqlException sqlException)
            {
                var failedTimeStorageException = new FailedTimeStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedTimeStorageException);
            }
        }

        private TimeDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var timeDependencyException = new TimeDependencyException(exception);
            this.loggingBroker.LogCritical(timeDependencyException);

            return timeDependencyException;
        }
    }
}
