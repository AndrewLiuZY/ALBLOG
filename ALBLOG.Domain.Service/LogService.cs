﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ALBLOG.Domain.Model;
using ALBLOG.Domain.Repository;
using ALBLOG.Domain.Service.Interface;

namespace ALBLOG.Domain.Service
{
    public class LogService : ILogService
    {

        private readonly LogRepository _repository;
        private readonly string _logId;

        public LogService()
        {
            this._repository = new LogRepository();
            this._logId = Guid.NewGuid().ToString();
        }

        public async Task Assert(string sessionId, string controllerName, string actionName, string IPAddress, string content, bool isAdmin = false)
        {
            await WriteAsync(LogType.Assert, sessionId, controllerName, actionName, IPAddress, content, isAdmin);
        }

        public async Task Error(string sessionId, string controllerName, string actionName, string IPAddress, string content, bool isAdmin = false)
        {
            await WriteAsync(LogType.Error, sessionId, controllerName, actionName, IPAddress, content, isAdmin);
        }

        public async Task Exception(string sessionId, string controllerName, string actionName, string IPAddress, string content, bool isAdmin = false)
        {
            await WriteAsync(LogType.Exception, sessionId, controllerName, actionName, IPAddress, content, isAdmin);
        }

        public async Task Log(string sessionId, string controllerName, string actionName, string IPAddress, string content, bool isAdmin = false)
        {
            await WriteAsync(LogType.Log, sessionId, controllerName, actionName, IPAddress, content, isAdmin);
        }

        public async Task Warning(string sessionId, string controllerName, string actionName, string IPAddress, string content, bool isAdmin = false)
        {
            await WriteAsync(LogType.Warning, sessionId, controllerName, actionName, IPAddress, content, isAdmin);
        }

        public async Task<IEnumerable<Log>> GetAllAsync(Expression<Func<Log, bool>> expression)
        {
            return await _repository.GetAllAsync(expression);
        }

        public async Task<Log> GetOneAsync(Expression<Func<Log, bool>> expression)
        {
            return await _repository.GetOneAsync(expression);
        }

        private async Task WriteAsync(LogType type, string sessionId, string controllerName, string actionName, string IPAddress, string content, bool isAdmin = false)
        {
            var log = new Log
            {
                LogId = _logId,
                Type = type,
                SessionId = sessionId,
                ControllerName = controllerName,
                ActionName = actionName,
                IPAddress = IPAddress,
                Content = content,
                IsAdmin = isAdmin,
                Date = DateTime.Now
            };
            await _repository.AddAsync(log);
        }

        public async Task<LogPage> GetPageAsync(int pageSize, int pageIndex)
        {
            return await GetPageAsync(_ => true, pageSize, pageIndex);
        }

        public async Task<LogPage> GetPageAsync(Expression<Func<Log, bool>> filter, int pageSize, int pageIndex)
        {
            var pageCount = await GetPageCountAsync(filter, pageSize);
            var allLogs = await _repository.GetAllAsync(filter);
            pageIndex = pageIndex <= 0 ? 1
                                       : pageIndex > pageCount ? pageCount
                                                               : pageIndex;
            var result = allLogs.Reverse()
                                .Skip(pageSize * (pageIndex - 1))
                                .Take(pageSize);
            var page = new LogPage
            {
                HaveLast = pageIndex > 1,
                HaveNext = pageIndex < pageCount,
                PageCount = pageCount,
                Index = pageIndex,
                Logs = result,
                Size = pageSize
            };
            return page;
        }

        public async Task<int> GetPageCountAsync(int pageSize)
        {
            return await GetPageCountAsync(_ => true, pageSize);
        }

        public async Task<int> GetPageCountAsync(Expression<Func<Log, bool>> filter, int pageSize)
        {
            var postCount = (await _repository.GetAllAsync(filter)).Count();
            var num = postCount / pageSize;
            var pageCount = postCount % pageSize > 0 ? num + 1
                                                     : num;
            return pageCount;
        }

        public Task<int> GetPageViewNum(DateTime date)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetPageViewNum(Expression<Func<Log, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetIpCount(DateTime date)
        {
            throw new NotImplementedException();
        }
    }
}