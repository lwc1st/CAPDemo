﻿using Dapper;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using Publisher.database;
using Publisher.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Publisher.Controllers
{
    [ApiController]
    [Route("api/publish")]
    public class PublishController : ControllerBase
    {
        ICapPublisher _capPublisher;
        ApplicationContext _context;

        public PublishController(ICapPublisher capPublisher, ApplicationContext context)
        {
            _capPublisher = capPublisher;
            _context = context;
        }

        [HttpGet("string")]
        public async Task<IActionResult> PublishString()
        {
            await _capPublisher.PublishAsync("sample.rabbitmq.demo.string", "this is text!");
            return Ok();
        }

        [HttpGet("dynamic")]
        public async Task<IActionResult> PublishDynamic()
        {
            await _capPublisher.PublishAsync("sample.rabbitmq.demo.dynamic", new
            {
                Name = "jiangyi",
                Age = 28
            });
            return Ok();
        }

        [HttpGet("object")]
        public async Task<IActionResult> PublishObject()
        {
            await _capPublisher.PublishAsync("sample.rabbitmq.demo.object", new Person
            {
                Name = "jiangyi222",
                Age = 30
            });
            return Ok();
        }

        [HttpGet("~/adonet/transaction")]
        public IActionResult PublishAdonetWithTransaction()
        {
            Debug.WriteLine($"PublishAdonetWithTransaction Invoked");
            using (var connection = new MySqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using (var transaction = connection.BeginTransaction(_capPublisher, true))
                {
                    var paras = new { Date = DateTime.Now, UserCode = Guid.NewGuid().ToString() };
                    //your business code
                    connection.Execute("INSERT INTO `user_info`(`name`, `age`, `eamil`, `user_code`) VALUES (@Name, 23, 'adonet@vip.com', @UserCode);",
                        new
                        {
                            Name = "tran_" + paras.Date,
                            UserCode = paras.UserCode
                        }
                        , (IDbTransaction)transaction.DbTransaction);
                    _capPublisher.Publish("sample.rabbitmq.mysql.adonet.transcation", paras, "sample.rabbitmq.mysql.adonet.callbackMethod");
                }
            }
            return Ok();
        }


        [NonAction]
        [CapSubscribe("sample.rabbitmq.mysql.adonet.callbackMethod")]
        public void PublishAdonetWithTransactionCallbackMethod(dynamic param)
        {
            Debug.WriteLine($"PublishAdonetWithTransactionCallbackMethod Invoked IsSussess:{param.IsSuccess} UserCode:{param.UserCode} Msg:{param.Msg}");
            if ((bool)param.IsSuccess)
            {
                //修改为成功状态
                Debug.WriteLine($"PublishAdonetWithTransactionCallbackMethod Invoked IsSuccess");
            }
            else
            {
                //修改为失败状态
                Debug.WriteLine($"PublishAdonetWithTransactionCallbackMethod Invoked Rollback");
                string userCode = param.UserCode;
                var user = _context.UserInfos.Single(x => x.UserCode == userCode);
                _context.UserInfos.Remove(user);
                _context.SaveChanges();
            }
        }

        [HttpGet("~/ef/transaction")]
        public async Task<IActionResult> PublishWithTranscation()
        {
            Debug.WriteLine($"PublishWithTranscation Invoked");
            using (var trans = _context.Database.BeginTransaction(_capPublisher, autoCommit: false))
            {
                var paras = new { Date = DateTime.Now, UserCode = Guid.NewGuid().ToString() };
                var user = new UserInfo
                {
                    UserCode = paras.UserCode,
                    Age = 22,
                    Eamil = "ef.transcation@vip.com",
                    Name = "emma_" + paras.Date
                };
                await _context.UserInfos.AddAsync(user);
                _capPublisher.Publish("sample.rabbitmq.mysql.ef.transcation", paras);

                _context.SaveChanges();
                trans.Commit();
            }
            return Ok();
        }
        [HttpGet("~/ef/autotransaction")]
        public async Task<IActionResult> PublishWithAutoTranscation()
        {
            Debug.WriteLine($"PublishWithAutoTranscation Invoked");
            using (var trans = _context.Database.BeginTransaction(_capPublisher, true))
            {
                var paras = new { Date = DateTime.Now, UserCode = Guid.NewGuid().ToString() };
                var user = new UserInfo
                {
                    UserCode = paras.UserCode,
                    Age = 22,
                    Eamil = "ef.transcation@vip.com",
                    Name = "emma_" + paras.Date
                };
                await _context.UserInfos.AddAsync(user);
                _capPublisher.Publish("sample.rabbitmq.mysql.ef.auto.transcation", paras);
                _context.SaveChanges();
                return Ok();
            }
        }
    }
    public class Person
    {
        public string Name { get; set; }
        public short Age { get; set; }
    }
}
