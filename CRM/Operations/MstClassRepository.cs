using CRM.BusinessModels.Tables;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Operations
{
    public class MstClassRepository
    {
        private readonly string _connectionString;
        public MstClassRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public int InsertClass(MstClass model)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_MstClass_All", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Flag", "INSERT");
                cmd.Parameters.AddWithValue("@Name", model.Name);
                cmd.Parameters.AddWithValue("@CreateBy", model.CreateBy);

                con.Open();
                int newId = Convert.ToInt32(cmd.ExecuteScalar());
                return newId;
            }
        }


        public string UpdateClass(MstClass model)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_MstClass_All", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Flag", "UPDATE");
                cmd.Parameters.AddWithValue("@ID", model.ID);
                cmd.Parameters.AddWithValue("@Name", model.Name);
                cmd.Parameters.AddWithValue("@UpdatedBy", model.UpdatedBy);

                con.Open();
                string result = cmd.ExecuteScalar()?.ToString();
                return result;
            }
        }


        public string DeleteClass(int id)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_MstClass_All", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Flag", "DELETE");
                cmd.Parameters.AddWithValue("@ID", id);

                con.Open();
                string result = cmd.ExecuteScalar()?.ToString();
                return result;
            }
        }

        public MstClass GetClassById(int id)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_MstClass_All", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Flag", "GETBYID");
                cmd.Parameters.AddWithValue("@ID", id);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                MstClass obj = null;
                if (dr.Read())
                {
                    obj = new MstClass
                    {
                        ID = Convert.ToInt32(dr["ID"]),
                        Name = dr["Name"].ToString(),
                        CreateBy = dr["CreateBy"].ToString(),
                        UpdatedBy = dr["UpdatedBy"].ToString(),
                        CreateDatetime = dr["CreateDatetime"] as DateTime?,
                        UpdateDatetime = dr["UpdateDatetime"] as DateTime?
                    };
                }
                return obj;
            }
        }


        public List<MstClass> GetAllClasses()
        {
            List<MstClass> list = new List<MstClass>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_MstClass_All", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Flag", "GETALL");

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    list.Add(new MstClass
                    {
                        ID = Convert.ToInt32(dr["ID"]),
                        Name = dr["Name"].ToString(),
                        CreateBy = dr["CreateBy"].ToString(),
                        UpdatedBy = dr["UpdatedBy"].ToString(),
                        CreateDatetime = dr["CreateDatetime"] as DateTime?,
                        UpdateDatetime = dr["UpdateDatetime"] as DateTime?
                    });
                }
            }
            return list;
        }
    }
}
