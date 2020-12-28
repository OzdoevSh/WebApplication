using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication.Models;
using NpgsqlTypes;
using Npgsql;

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private shopEntities db = new shopEntities();

        public ActionResult Query1(string queryText1)
        {
            var result = db.deposits
                         .ToArray()
                .Where(c => c.client.firstname.ToLower().Contains(queryText1.ToLower()));
            return View(result);
        }

        public ActionResult Query2(int from, int to)
        {
            var result = db.deposits
                         .ToArray()
                .Where(c => c.value >= from && c.value <= to);
            return View(result);
        }

        [HttpPost]
        public ActionResult Query3(string search)
        {
            List<Query3Result> result = new List<Query3Result>();
            using (var db = new shopEntities())
            {
                string sql = @"SELECT id, name, (price * count) as summary FROM product GROUP BY (id, name)";
                result = db.Database.SqlQuery<Query3Result>(sql).ToList();
            }
            TempData["result"] = result;
            TempData.Keep();
            return View(TempData["result"]);
        }

        [HttpPost]
        public ActionResult Query4(string regex)
        {
            List<Query4Result> result = new List<Query4Result>();
            using (var db = new shopEntities())
            {
                var param = new NpgsqlParameter("@regex", regex)
                {
                    NpgsqlDbType = NpgsqlDbType.Varchar
                };
                string sql = @"SELECT firstName, lastName, patronymic, address, email, phone, p.name AS productName, p.price AS productPrice, bi.count AS count FROM client c 
                               INNER JOIN basket b ON c.id = b.client_id 
                               LEFT OUTER JOIN basketItem bi ON b.id = bi.basket_id 
                               LEFT OUTER JOIN product p ON p.id = bi.product_id
                               WHERE c.firstName LIKE '%'|| @regex ||'%'";
                result = db.Database.SqlQuery<Query4Result>(sql, param).ToList();
            }
            TempData["result"] = result;
            TempData.Keep();
            return View(TempData["result"]);
        }

        [HttpPost]
        public ActionResult Query5(string digit)
        {
            List<Query5Result> result = new List<Query5Result>();
            using (var db = new shopEntities())
            {
                var param = new NpgsqlParameter("@digit", digit)
                {
                    NpgsqlDbType = NpgsqlDbType.Varchar
                };
                string sql = @"SELECT ROUND(AVG(d.value)) FROM deposit d JOIN client c ON c.id = d.client_id WHERE c.phone LIKE '%' || @digit || '%'";
                result = db.Database.SqlQuery<Query5Result>(sql, param).ToList();
            }
            TempData["result"] = result;
            TempData.Keep();
            return View(TempData["result"]);
        }

        [HttpPost]
        public ActionResult Query6(string price, string count)
        {
            List<Query6Result> result = new List<Query6Result>();
            using (var db = new shopEntities())
            {
                var param1 = new NpgsqlParameter("@price", Convert.ToInt32(price))
                {
                    NpgsqlDbType = NpgsqlDbType.Integer
                };
                var param2 = new NpgsqlParameter("@count", Convert.ToInt32(count))
                {
                    NpgsqlDbType = NpgsqlDbType.Integer
                };
                string sql = @"SELECT p.name, p.price, p.count FROM product p WHERE price > @price 
                               UNION 
                               SELECT pu.productName, pu.productPrice, count FROM purchase pu WHERE pu.count > @count";
                result = db.Database.SqlQuery<Query6Result>(sql, param1, param2).ToList();
            }
            TempData["result"] = result;
            TempData.Keep();
            return View(TempData["result"]);
        }

        [HttpPost]
        public ActionResult Query7()
        {
            List<Query7Result> result = new List<Query7Result>();
            using (var db = new shopEntities())
            {
                string sql = @"SELECT c1.id, c1.name AS name, c2.name AS parentName, c2.id AS parentId FROM category c1 
                               JOIN category c2 ON c1.parentid = c2.id 
                               WHERE c1.parentid IS NOT NULL";
                result = db.Database.SqlQuery<Query7Result>(sql).ToList();
            }
            TempData["result"] = result;
            TempData.Keep();
            return View(TempData["result"]);
        }

        [HttpPost]
        public ActionResult Query8(string city)
        {
            List<Query8Result> result = new List<Query8Result>();
            using (var db = new shopEntities())
            {
                var param = new NpgsqlParameter("@city", city)
                {
                    NpgsqlDbType = NpgsqlDbType.Varchar
                };
                string sql = @"SELECT c.firstname, c.lastname, c.patronymic, c.email, c.address, c.phone, c.login, d.value AS deposit, p.productname, p.productprice, p.count 
                               FROM purchase p JOIN client c ON p.client_id = c.id 
                               JOIN deposit d ON c.id = d.client_id 
                               WHERE c.address = @city";
                result = db.Database.SqlQuery<Query8Result>(sql, param).ToList();
            }
            TempData["result"] = result;
            TempData.Keep();
            return View(TempData["result"]);
        }

        [HttpPost]
        public ActionResult Query9(string name)
        {
            List<Query6Result> result = new List<Query6Result>();
            using (var db = new shopEntities())
            {
                var param = new NpgsqlParameter("@name", name)
                {
                    NpgsqlDbType = NpgsqlDbType.Varchar
                };

                string sql = @"SELECT p.name, p.price, p.count FROM product p 
                               JOIN product_category pc ON pc.product_id = p.id 
                               JOIN category c ON pc.category_id = c.id WHERE c.name = @name";
                result = db.Database.SqlQuery<Query6Result>(sql, param).ToList();
            }
            TempData["result"] = result;
            TempData.Keep();
            return View(TempData["result"]);
        }

        [HttpPost]
        public ActionResult Query10(string name)
        {
            List<Query8Result> result = new List<Query8Result>();
            using (var db = new shopEntities())
            {
                string sql = @"SELECT c.firstname, c.lastname, c.patronymic, c.email, c.address, c.phone, c.login, d.value AS deposit, p.name AS productname, p.price AS productprice, bi.count 
                               FROM client c 
                               INNER JOIN deposit d ON c.id = d.client_id 
                               INNER JOIN basket b ON c.id = b.client_id 
                               LEFT OUTER JOIN basketItem bi ON b.id = bi.basket_id 
                               JOIN product p ON bi.product_id = p.id 
                               ORDER BY c.login";
                result = db.Database.SqlQuery<Query8Result>(sql).ToList();
            }
            TempData["result"] = result;
            TempData.Keep();
            return View(TempData["result"]);
        }

        [HttpPost]
        public ActionResult Query11(DateTime from, DateTime to)
        {
            var paramFrom = new NpgsqlParameter("@from", from)
            {
                NpgsqlDbType = NpgsqlDbType.Date
            };
            var paramTo = new NpgsqlParameter("@to", to)
            {
                NpgsqlDbType = NpgsqlDbType.Date
            };
            List<Query11Result> result = new List<Query11Result>();
            using (var db = new shopEntities())
            {
                string sql = @"SELECT * FROM purchase pu 
                               JOIN client c ON pu.client_id = c.id 
                               JOIN product p ON pu.product_id = p.id 
                               WHERE date BETWEEN @from AND @to";
                result = db.Database.SqlQuery<Query11Result>(sql, paramFrom, paramTo).ToList();
            }
            TempData["result"] = result;
            TempData.Keep();
            return View(TempData["result"]);
        }

        public ActionResult Queries()
        {
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}