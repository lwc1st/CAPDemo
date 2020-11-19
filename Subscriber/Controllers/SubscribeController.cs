using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using Subscriber.Database;
using Subscriber.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Subscriber.Controllers
{
    [ApiController]
    public class SubscribeController : Controller
    {
        ICapPublisher _capPublisher;
        ApplicationContext _context;
        public SubscribeController(ICapPublisher capPublisher, ApplicationContext context)
        {
            _capPublisher = capPublisher;
            _context = context;
        }
        [NonAction]
        [CapSubscribe("sample.rabbitmq.demo.string")]
        public void SubscriberString(string text)
        {
            Debug.WriteLine($"【SubscriberString】Subscriber invoked, Info: {text}");
        }

        [NonAction]
        [CapSubscribe("sample.rabbitmq.demo.dynamic")]
        public void SubscriberDynamic(dynamic person)
        {
            Debug.WriteLine($"【SubscriberDynamic】Subscriber invoked, Info: {person.Name} {person.Age}");
        }

        [NonAction]
        [CapSubscribe("sample.rabbitmq.demo.object")]
        public void SubscriberObject(Person person)
        {
            Debug.WriteLine($"【SubscriberObject】Subscriber invoked, Info: {person.Name} {person.Age}");
        }
        [NonAction]
        [CapSubscribe("sample.rabbitmq.mysql.adonet.transcation")]
        public async Task<object> SubscribeAdonetWithTransaction(dynamic paras)
        {
            Debug.WriteLine($"【SubscribeAdonetWithTransaction】Subscriber invoked, Info: {paras.UserCode} {paras.Date}");
            var isSuccess = false;
            var msg = "";
            try
            {
                using (var trans = _context.Database.BeginTransaction(_capPublisher, autoCommit: false))
                {
                    var order = new OrderInfo
                    {
                        OrderCode = Guid.NewGuid(),
                        UserCode = paras.UserCode
                    };
                    await _context.OrderInfos.AddAsync(order);
                    _context.SaveChanges();
                    trans.Commit();
                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                throw ex;
            }
            return new { Msg = msg, IsSuccess = isSuccess, UserCode = paras.UserCode };
        }

        [NonAction]
        [CapSubscribe("sample.rabbitmq.mysql.ef.transcation")]
        public async Task SubscribeWithTranscation(dynamic paras)
        {
            Debug.WriteLine($"【SubscribeWithTranscation】Subscriber invoked, Info: {paras.UserCode} {paras.Date}");
            using (var trans = _context.Database.BeginTransaction(_capPublisher, autoCommit: false))
            {
                var order = new OrderInfo
                {
                    OrderCode = Guid.NewGuid(),
                    UserCode = paras.UserCode
                };
                await _context.OrderInfos.AddAsync(order);
                _context.SaveChanges();
                trans.Commit();
            }
        }
        [NonAction]
        [CapSubscribe("sample.rabbitmq.mysql.ef.auto.transcation")]
        public async Task SubscribeWithAutoTranscation(dynamic paras)
        {
            Debug.WriteLine($"【SubscribeWithAutoTranscation】Subscriber invoked, Info: {paras.UserCode} {paras.Date}");
            using (var trans = _context.Database.BeginTransaction(_capPublisher, autoCommit: true))
            {
                var order = new OrderInfo
                {
                    OrderCode = Guid.NewGuid(),
                    UserCode = paras.UserCode
                };
                await _context.OrderInfos.AddAsync(order);
                _context.SaveChanges();
                trans.Commit();
            }
        }
    }
    public class Person
    {
        public string Name { get; set; }
        public short Age { get; set; }
    }
}
