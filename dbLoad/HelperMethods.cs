using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Xml;

namespace dbLoad
{
    class HelperMethods
    {

        static Dictionary<string, string> tag_table = new Dictionary<string, string>
        {

            { "vendor.model", "VendorModels" },
            { "tour", "Tours"},
            {"event-ticket", "EventTickets" },
            {"book", "Books" },
            {"audiobook", "audiobooks" },
            {"artist.title", "ArtistTitle" }

        };

        public static void addToDb(Dictionary<string, StringBuilder> queryStrings, string type)
        {
            string connectionString;
            SqlConnection conn;

            connectionString = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={Directory.GetCurrentDirectory()}\OffersDb.mdf;Integrated Security=True";

            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();

                //формируем строку запроса

                string query = $"insert into {tag_table[type]} ({queryStrings["sb_columns"]}) values ({queryStrings["sb_values"]})";
                SqlCommand command = new SqlCommand(query, conn);

                command.ExecuteNonQuery();
                command.Dispose();

                conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static Dictionary<string, StringBuilder> getQueryStrings(XmlNodeList list)
        {
            StringBuilder sb_values = new StringBuilder($"'{Guid.NewGuid()}'"); //формирование строки для значений колонок

            StringBuilder sb_columns = new StringBuilder("Id"); //формирование строки колонок

            foreach (XmlElement element in list)
            {
                sb_columns.Append(","); //разделитель запроса
                if (sb_columns.ToString().Contains(element.Name)) //если есть повторяющийся тэг
                {
                    sb_columns.Append($"{element.Name}2"); //записываем его в другую колонку
                }
                else
                {
                    sb_columns.Append(element.Name);
                }

                sb_values.Append(", N'"); //добавление пуктуации запросу и символа для добавления кириллицы
                sb_values.Append(element.InnerText.Replace("'", "''")); //если тескт содержит одинарные кавычки
                                                                        //заменить его на две кавычки для корректной работы запроса
                sb_values.Append("'"); //закрываем кавычкой значение 
            }
            return new Dictionary<string, StringBuilder>
            {
                {"sb_columns", sb_columns },
                {"sb_values", sb_values }
            };
        }
    }
}
