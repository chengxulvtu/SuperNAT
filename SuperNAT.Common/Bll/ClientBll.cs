﻿using Dapper;
using SuperNAT.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SuperNAT.Common.Bll
{
    public class ClientBll : BaseBll<Client>
    {
        public ReturnResult<Client> GetOne(string secret)
        {
            var rst = new ReturnResult<Client>() { Message = "暂无记录" };

            try
            {
                rst.Data = conn.QueryFirstOrDefault<Client>("select * from client where secret=@secret", new { secret });
                if (rst.Data != null)
                {
                    rst.Result = true;
                    rst.Message = "获取成功";
                }
            }
            catch (Exception ex)
            {
                rst.Message = $"获取失败：{ex.InnerException ?? ex}";
                Log4netUtil.Error($"{ex.InnerException ?? ex}");
            }

            return rst;
        }

        public ReturnResult<List<Client>> GetList(Client model)
        {
            var rst = new ReturnResult<List<Client>>() { Message = "暂无记录" };

            try
            {
                rst.Data = conn.Query<Client>(@"SELECT
	                                                t1.*, t2.user_name
                                                FROM
	                                                client t1
                                                LEFT JOIN `user` t2 ON t1.user_id = t2.user_id", model).ToList();
                if (rst.Data != null)
                {
                    rst.Result = true;
                    rst.Message = "获取成功";
                }
            }
            catch (Exception ex)
            {
                rst.Message = $"获取失败：{ex.InnerException ?? ex}";
                Log4netUtil.Error($"{ex.InnerException ?? ex}");
            }

            return rst;
        }
    }
}
