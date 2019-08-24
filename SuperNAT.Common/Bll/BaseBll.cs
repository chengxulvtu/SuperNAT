﻿using Dapper;
using MySql.Data.MySqlClient;
using SuperNAT.Common.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperNAT.Common.Bll
{
    public class BaseBll<T> : IDisposable
    {
        public MySqlConnection conn;
        public BaseBll()
        {
            try
            {
                var connStr = "server=127.0.0.1;port=3306;User Id=root;Password=123456;Database=nat;pooling=false;character set=utf8;SslMode=none;";
                conn = new MySqlConnection(connStr);
                conn.Open();
            }
            catch (Exception ex)
            {
                Log4netUtil.Error("在BaseDal中打开连接时出错：" + ex);
            }
        }

        public ReturnResult<bool> Add(T model)
        {
            var rst = new ReturnResult<bool>() { Message = "添加失败" };

            try
            {
                if (conn.Insert(model) > 0)
                {
                    rst.Result = true;
                    rst.Message = "添加成功";
                }
            }
            catch (Exception ex)
            {
                rst.Message = $"添加失败：{ex.InnerException ?? ex}";
                Log4netUtil.Error($"{ex.InnerException ?? ex}");
            }

            return rst;
        }

        public ReturnResult<bool> Update(T model)
        {
            var rst = new ReturnResult<bool>() { Message = "更新失败" };

            try
            {
                if (conn.Update(model) > 0)
                {
                    rst.Result = true;
                    rst.Message = "更新成功";
                }
            }
            catch (Exception ex)
            {
                rst.Message = $"更新失败：{ex.InnerException ?? ex}";
                Log4netUtil.Error($"{ex.InnerException ?? ex}");
            }

            return rst;
        }

        public ReturnResult<bool> Delete(T model)
        {
            var rst = new ReturnResult<bool>() { Message = "删除失败" };

            try
            {
                if (conn.Delete(model) > 0)
                {
                    rst.Result = true;
                    rst.Message = "删除成功";
                }
            }
            catch (Exception ex)
            {
                rst.Message = $"删除失败：{ex.InnerException ?? ex}";
                Log4netUtil.Error($"{ex.InnerException ?? ex}");
            }

            return rst;
        }

        public ReturnResult<T> GetOne(IModel model)
        {
            var rst = new ReturnResult<T>() { Message = "暂无记录" };

            try
            {
                rst.Data = conn.Get<T>(model.id);
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

        public ReturnResult<List<T>> GetList()
        {
            var rst = new ReturnResult<List<T>>() { Message = "暂无记录" };

            try
            {
                rst.Data = conn.GetList<T>().ToList();
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

        public void Dispose()
        {
            try
            {
                if (conn != null && conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
                conn?.Dispose();
            }
            catch (Exception ex)
            {
                Log4netUtil.Error("在BaseBll中关闭连接时出错：" + ex);
            }
        }
    }
}
