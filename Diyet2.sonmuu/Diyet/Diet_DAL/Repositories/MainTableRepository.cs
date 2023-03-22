﻿using Diet_Model.Entity;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Core.Mapping;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Diet_DAL.Repositories
{
    public class MainTableRepository
    {
        AppDbContext dbContext;
        public MainTableRepository()
        {
            dbContext = new AppDbContext();
        }
        public bool Insert(MainTable mainTable)
        {
            dbContext.MainTables.Add(mainTable);
            return dbContext.SaveChanges() > 0;
        }

        public List<MainTable> GetListMainTablesByID(int mainTableid)
        {
            return dbContext.MainTables.Where(a => a.ID == mainTableid).ToList();
        }

        public List<MainTable> GetAllMainTables()
        {
            return dbContext.MainTables.OrderBy(a => a.ID).ToList();
        }

        public MainTable GetMainTableById(int mainTableId)
        {
            return dbContext.MainTables.Where(a => a.ID == mainTableId).FirstOrDefault();
        }

        public List<MainTable> GetListMainTablesByUserID(int userID)
        {
            return dbContext.MainTables.Where(a => a.UserID == userID).ToList();
        }

        public List<MainTable> GetListMainTablesByMealID(int mealID)
        {
            return dbContext.MainTables.Where(a => a.MealID == mealID).ToList();
        }

        public List<MainTable> GetListMainTablesByNutrientID(int nutrientID)
        {
            return dbContext.MainTables.Where(a => a.NutrientID == nutrientID).ToList();
        }

        public MainTable GetMainTablesByUserID(int userID)
        {
            return dbContext.MainTables.Where(a => a.UserID == userID).FirstOrDefault();
        }

        public MainTable GetMainTablesByMealID(int mealID)
        {
            return dbContext.MainTables.Where(a => a.MealID == mealID).FirstOrDefault();
        }

        public MainTable GetMainTablesByNutrientID(int nutrientID)
        {
            return dbContext.MainTables.Where(a => a.NutrientID == nutrientID).FirstOrDefault();
        }
        public void GetCaloriesbyUserID(int userid, Chart c1, DateTime d1, DateTime d2)
        {

            var adminrapor = (from m in dbContext.Meals
                              join mt in dbContext.MainTables on m.ID equals mt.MealID
                              join u in dbContext.Users on mt.UserID equals u.ID
                              join n in dbContext.Nutrients on mt.NutrientID equals n.ID
                              where u.ID == userid && m.CreateTime >= d1 || m.CreateTime <= d2
                              group m by m.CreateTime into g
                              select new
                              {
                                  CreateTime = g.Key.ToString(),
                                  Calories = dbContext.Nutrients.Sum(n => n.Calories),
                              }).ToList();

            c1.DataSource = adminrapor;
            c1.Series["Günlük Kalori"].XValueMember = "CreateTime";
            c1.Series["Günlük Kalori"].YValueMembers = "Calories";
            c1.DataBind();

        }

        public void GetDatebyUserId(int userid, ComboBox comboBox)
        {
            var dates = (from m in dbContext.Meals
                         join mt in dbContext.MainTables on m.ID equals mt.MealID
                         join u in dbContext.Users on mt.UserID equals u.ID
                         where u.ID == userid
                         select new
                         {
                             CreateTime = m.CreateTime,
                         }).Distinct().ToList();

            comboBox.DataSource = dates;
            comboBox.DisplayMember = "CreateTime";

        }

        public MainTable CalculateTotalCal(DateTime d1, int userid)
        {
            var list = dbContext.MainTables.Where(a => a.Meal.CreateTime == d1 && a.User.ID == userid).FirstOrDefault();

            return list;
        }
        public double CalculateTotalCalTurnList(DateTime d1, int userid)
        {
            var list = dbContext.MainTables.Where(a => a.Meal.CreateTime == d1 && a.User.ID == userid).Select(a => new
            {
                a.TotalCalorie,
                
            }).ToList();
            double toplam = 0;
            foreach (var item in list)
            {
                toplam += item.TotalCalorie;
            }
            return toplam;
        }
    }
}
