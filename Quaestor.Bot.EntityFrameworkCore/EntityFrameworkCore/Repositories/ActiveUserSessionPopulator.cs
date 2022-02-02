using Quaestor.Bot.UserSessions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Quaestor.Bot.EntityFrameworkCore.Repositories
{
    public class ActiveUserSessionPopulator
    {
        public List<ActiveUserSession> CreateList(SqlDataReader reader)
        {
            List<ActiveUserSession> result = new List<ActiveUserSession>();
            while (reader.Read())
            {
                ActiveUserSession item = new ActiveUserSession()
                {
                    UserSessionDetailId = Convert.ToInt32(reader["UserSessionDetailId"]),
                    MarketName = Convert.ToString(reader["MarketName"]),
                    RebuyValue = Convert.ToDecimal(reader["RebuyValue"]),
                    RebuyNumber = Convert.ToInt32(reader["RebuyNumber"])
                };
                result.Add(item);
            }

            return result;
        }
    }
}
