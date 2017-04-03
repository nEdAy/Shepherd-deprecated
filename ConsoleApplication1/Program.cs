using MvcApplication1.BLL;
using MvcApplication1.Model;
using MvcApplication1.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            _UserBLL bll = new _UserBLL();
            List<Wheres> whs = new List<Wheres>() { new Wheres("objectId", "like", "%1%") };
            var dir = bll.QuerySingleByWheres(whs);
            //object c = new { name = "1", id = "2" };
            //var props = c.GetType().GetProperties();
            //foreach (var prop in props)
            //{
            //    string name = prop.Name;
            //    object value = prop.GetValue(c);

            //}
             
            //Random ran = new Random();
            //for (int i=0; i < 20; i++) {
                
            //    int num=ran.Next(-3, 3);
            //    Console.WriteLine(num);
            //}

            string initPassword = "abcd1234";
            string str = initPassword.Md5();
            string str1 = (initPassword.Md5() + "af5e02e5-4ceb-4cf8-81d2-bcca13a24952").Md5();
            string str2 = (initPassword.Md5() + "5efb2ff4-fee6-46dc-a0e1-aaa35fea9bce").Md5();
            string str3 = (initPassword.Md5() + "8879e932-f4d7-4766-9d93-f95c53c90e6e").Md5();
            //5efb2ff4-fee6-46dc-a0e1-aaa35fea9bce
            //8879e932-f4d7-4766-9d93-f95c53c90e6e
            Console.WriteLine(str);
            Console.WriteLine(str1);
            Console.WriteLine(str2);
            Console.WriteLine(str3);
                //string raw = "abcd1234";
                //string jiami=raw.Md5();

                //Console.WriteLine(jiami);
                //string str = jiami + "e6d21995-50c3-4aac-81ec-a99d72f2de7f";
                //string jiami1=str.Md5();
                //Console.WriteLine(jiami1);
                //_UserBLL bll = new _UserBLL();

                //Dictionary<string, string[]> dir = new Dictionary<string, string[]>(){
                //   {"_User",new string[]{"createdAt","password"}}
                //};
                //var models=bll.QueryListX(0, 3, dir);
                //string str = JsonHelper.Serialize(models);
                //Console.WriteLine(str);

                //var model=bll.QuerySingleByIdX(@"2d1d6669-059e-4931-9365-f964f1ce93c4", dir);
                //string str = JsonHelper.Serialize(model);
                //Console.WriteLine(str);
                //RestClient client = new RestClient("http://192.168.191.1");

                //List<Columns> li = new List<Columns>();

                //Columns col = new Columns();
                //col.key = "aa";
                //col.value = 2;

                //Columns col1 = new Columns();
                //col1.key = "ab";
                //col1.value = 2;

                //li.Add(col);
                //li.Add(col1);



                //Dictionary<string, object> dic = new Dictionary<string, object>() { { "22", 3 }, { "23", 3 } };
                //string[] arr = { "1", "2", "3" };
                //string str = JsonHelper.Serialize(li);
                //string str1 = JsonHelper.Serialize(dic);
                //string str2 = JsonHelper.Serialize(arr);
                //Console.WriteLine(str);
                //Console.WriteLine(str1);
                //Console.WriteLine(str2);
                //string[] arr1 = JsonHelper.Deserialize<string[]>(str2);
                //Console.WriteLine(arr1[0]);


                //string[] cc = null;
                //object cdd = cc;
                //string[] ee = (string[])cdd;
                //Console.WriteLine(ee);
                //Console.WriteLine("11");
                //#region Get 方式请求列表
                //string str = client.Get("api/values");

                //Console.WriteLine(str);
                //#endregion

                //#region Get 方式请求id对应的数据
                //string strGetById = client.Get("api/values/2");

                //Console.WriteLine(strGetById);
                //#endregion

                //#region Post 方式 添加数据

                //string postUri = "api/users?code=333";

                //string userJson = @"{""username"":""11131323"",""password"":""12""}";

                //string postResponse = client.Post(userJson, postUri);

                //Console.WriteLine(postResponse);
                //#endregion

                //#region Delete

                //string deleteUri = "api/values/3";
                //string deleteResponse = client.Delete(deleteUri);

                //Console.WriteLine(deleteResponse);
                //#endregion

                //#region Put
                //string putUri = "api/values/123";

                //string userJson3 = @"{""Id"":11123,""Age"":12,""UserName"":""111""}";

                //string putResponse = client.Post(userJson3, putUri);

                //Console.WriteLine(putResponse);
                //#endregion

                Console.ReadKey();
        }
    }
}
