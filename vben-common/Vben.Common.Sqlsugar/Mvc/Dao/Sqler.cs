namespace Vben.Common.Sqlsugar.Mvc.Dao;

//查询用的SqlHelper 配合原生sql使用
public class Sqler
{
    private string fromClause = "";
    private string whereClause = "";
    private string orderClause = "";
    private string selectClause = "";
    private string groupClause = "";
    private int panum = 1;
    private int pasiz = 10;
    private int autoType = 1;
    private Dictionary<string, object> parameters = new Dictionary<string, object>(); // 参数清单


    public Sqler()
    {
    }

    public Sqler(string table, int panum, int pasiz)
    {
        this.selectClause = "SELECT t.id,t.name";
        this.fromClause = " FROM " + table + " t";
        if (panum != 0)
        {
            this.panum = panum;
        }

        if (pasiz != 0)
        {
            this.pasiz = pasiz;
        }
    }

    public Sqler(string fields, string table, int panum, int pasiz)
    {
        this.selectClause = "SELECT " + fields;
        this.fromClause = " FROM " + table + " t";
        if (panum != 0)
        {
            this.panum = panum;
        }

        if (pasiz != 0)
        {
            this.pasiz = pasiz;
        }
    }


    public Sqler(string table)
    {
        this.selectClause = "SELECT t.id,t.name";
        this.fromClause = " FROM " + table + " t";
    }

    public Sqler(string fields, string table)
    {
        this.selectClause = "SELECT " + fields;
        this.fromClause = " FROM " + table + " t";
    }

    public void addSelect(string fields)
    {
        selectClause += "," + fields;
    }

    public Sqler addInnerJoin(string fields, string table, string condition)
    {
        if (fields != "")
        {
            selectClause += "," + fields;
        }

        fromClause += " INNER JOIN " + table + " ON " + condition;
        return this;
    }

    public Sqler addLeftJoin(string fields, string table, string condition)
    {
        if (fields != "")
        {
            selectClause += "," + fields;
        }

        fromClause += " Left JOIN " + table + " ON " + condition;
        return this;
    }

    public Sqler addWhere(string condition)
    {
        if (whereClause.Length == 0)
        {
            whereClause = " WHERE " + condition;
        }
        else
        {
            whereClause += " AND " + condition;
        }

        return this;
    }
    
    public Sqler addWhere(string condition, string pname, object value)
    {
        if (whereClause.Length == 0)
        {
            whereClause = " WHERE " + condition;
        }
        else
        {
            whereClause += " AND " + condition;
        }
        parameters.Add(pname, value);
        return this;
    }

    public Sqler addEqual(string name, object value)
    {
        string pname = name.Substring(name.LastIndexOf(".") + 1);
        if (!string.IsNullOrEmpty(value + ""))
        {
            if (whereClause.Length == 0)
            {
                whereClause = " WHERE " + name + " = @" + pname;
            }
            else
            {
                whereClause += " AND " + name + " = @" + pname;
            }

            parameters.Add(pname, value);
        }

        return this;
    }

    public Sqler addLike(string name, object value)
    {
        string pname = name.Substring(name.LastIndexOf(".") + 1);
        if (!string.IsNullOrEmpty(value + ""))
        {
            if (whereClause.Length == 0)
            {
                whereClause = " WHERE " + name + " like @" + pname;
            }
            else
            {
                whereClause += " AND " + name + " like @" + pname;
            }

            parameters.Add(pname, "%" + ("" + value).Trim() + "%");
        }

        return this;
    }

    public Sqler addGtStringDate(string name, object value)
    {
        string pname = name.Substring(name.LastIndexOf(".") + 1);
        if (!string.IsNullOrEmpty(value + ""))
        {
            if (whereClause.Length == 0)
            {
                whereClause = " WHERE " + name + ">=@" + pname;
            }
            else
            {
                whereClause += " AND " + name + ">=@" + pname;
            }

            parameters.Add(pname, value);
        }

        return this;
    }

    public Sqler addGtDate(string name, object value, string dbType)
    {
        string pname = name.Substring(name.LastIndexOf(".") + 1);
        if ("ORACLE" == dbType)
        {
            if (!string.IsNullOrEmpty(value + ""))
            {
                if (whereClause.Length == 0)
                {
                    whereClause = " WHERE " + name + ">=to_date(@" + pname + ",'yyyy-MM-dd')";
                }
                else
                {
                    whereClause += " AND " + name + ">=to_date(@" + pname + ",'yyyy-MM-dd')";
                }

                parameters.Add(pname, value);
            }
        }
        else if ("MYSQL" == dbType)
        {
            if (!string.IsNullOrEmpty(value + ""))
            {
                if (whereClause.Length == 0)
                {
                    whereClause = " WHERE unix_timestamp(" + name + ") >=unix_timestamp(@" + pname + ")";
                }
                else
                {
                    whereClause += " AND unix_timestamp(" + name + ")  >=unix_timestamp(@" + pname + ")";
                }

                parameters.Add(pname, value);
            }
        }

        return this;
    }

    public Sqler addLtDate(string name, object value, string dbType)
    {
        string pname = name.Substring(name.LastIndexOf(".") + 1);
        if ("ORACLE" == dbType)
        {
            if (!string.IsNullOrEmpty(value + ""))
            {
                if (whereClause.Length == 0)
                {
                    whereClause = " WHERE " + name + "<to_date(@" + pname + ",'yyyy-MM-dd')+1";
                }
                else
                {
                    whereClause += " AND " + name + "<to_date(@" + pname + ",'yyyy-MM-dd')+1";
                }

                parameters.Add(pname, value);
            }
        }
        else
        {
            if (!string.IsNullOrEmpty(value + ""))
            {
                if (whereClause.Length == 0)
                {
                    whereClause = " WHERE unix_timestamp(" + name + ") <=unix_timestamp(@" + pname + ")";
                }
                else
                {
                    whereClause += " AND unix_timestamp(" + name + ") <=unix_timestamp(@" + pname + ")";
                }

                parameters.Add(pname, value + " 23:59:59");
            }
        }

        return this;
    }

    public Sqler addLtStringDate(string name, object value)
    {
        string pname = name.Substring(name.LastIndexOf(".") + 1);
        if (!string.IsNullOrEmpty(value + ""))
        {
            if (whereClause.Length == 0)
            {
                whereClause = " WHERE " + name + "<=@" + pname;
            }
            else
            {
                whereClause += " AND " + name + "<=@" + pname;
            }

            parameters.Add(name, value);
        }

        return this;
    }

    public Sqler addOrder(string propertyName)
    {
        if (orderClause.Length == 0)
        {
            orderClause = " ORDER BY " + propertyName;
        }
        else
        {
            orderClause += ", " + propertyName;
        }

        return this;
    }

    public Sqler addDescOrder(string propertyName)
    {
        if (orderClause.Length == 0)
        {
            orderClause = " ORDER BY " + propertyName + " DESC";
        }
        else
        {
            orderClause += ", " + propertyName + " DESC";
        }

        return this;
    }

    public Sqler addGroup(string gclause)
    {
        groupClause = " GROUP BY " + gclause;
        return this;
    }

    public Sqler addJoin(string joinClause)
    {
        this.fromClause += " " + joinClause;
        return this;
    }


    public Sqler selectCinfo()
    {
        addLeftJoin("oo1.name as crman,t.crtim", "sys_org oo1", "oo1.id=t.crmid");
        return this;
    }


    public Sqler selectCUinfo()
    {
        addLeftJoin("oo1.name as crman,t.crtim", "sys_org oo1", "oo1.id=t.crmid");
        addLeftJoin("oo2.name as upman,t.uptim", "sys_org oo2", "oo2.id=t.upmid");
        return this;
    }


    //get and set-------------------------------------------

    public string getSql()
    {
        return selectClause + fromClause + whereClause + groupClause + orderClause;
    }

    public string getLowerCaseSql()
    {
        //字段转成小写
        StringBuilder changeSelectClause = new StringBuilder();
        String[] strArr = selectClause.Split(",");
        lowerCaseSelect(changeSelectClause, strArr);
        changeSelectClause.Remove(changeSelectClause.Length - 1, 1);
        // changeSelectClause.DeleteCharAt(changeSelectClause.Length - 1);
        return changeSelectClause.ToString() + fromClause + whereClause + groupClause + orderClause;
    }

    public Sqler getNewGroupSqler(String groupClause)
    {
        Sqler sqler = new Sqler("");
        sqler.fromClause = this.fromClause;
        sqler.whereClause = this.whereClause;
        sqler.groupClause = " group by " + groupClause;
        return sqler;
    }


    public String getSizeSql()
    {
        if ("" == groupClause)
        {
            return "SELECT count(1)" + fromClause + whereClause;
        }
        else
        {
            return "SELECT count(1) FROM (SELECT 1 " + fromClause + whereClause + groupClause + ") SSZZ";
        }
    }

    public String getMysqlPagingSql()
    {
        int fromIndex = pasiz * (panum - 1);
        return selectClause + fromClause + whereClause + groupClause + orderClause + " limit " + fromIndex + "," +
               pasiz;
    }

    public String getOraclePagingSql()
    {
        int rownum = panum * pasiz;
        int rn = (panum - 1) * pasiz;
        return " SELECT * FROM (SELECT PPGG.*, ROWNUM RN FROM (" + selectClause + fromClause + whereClause +
               groupClause + orderClause + ") PPGG  WHERE ROWNUM <= " + rownum + ")  WHERE RN > " + rn;
        //        return " SELECT * FROM (SELECT PPGG.*, ROWNUM RN FROM (" + changeSelectClause.toString() + fromClause + whereClause+groupClause + orderClause + ") PPGG  WHERE ROWNUM <= " + rownum + ")  WHERE RN > " + rn;
    }

    public String getOraclePagingLowerCaseSql()
    {
        //字段转成小写
        StringBuilder changeSelectClause = new StringBuilder();
        String[] strArr = selectClause.Split(",");
        lowerCaseSelect(changeSelectClause, strArr);
        changeSelectClause.Remove(changeSelectClause.Length - 1, 1);
        // changeSelectClause.deleteCharAt(changeSelectClause.Length - 1);
        //分页
        int rownum = panum * pasiz;
        int rn = (panum - 1) * pasiz;
        return " SELECT * FROM (SELECT PPGG.*, ROWNUM RN FROM (" + changeSelectClause.ToString() + fromClause +
               whereClause + groupClause + orderClause + ") PPGG  WHERE ROWNUM <= " + rownum + ")  WHERE RN > " +
               rn;
    }

    private void lowerCaseSelect(StringBuilder changeSelectClause, String[] strArr)
    {
        foreach (var aStrArr in strArr)
        {
            if (aStrArr.Contains("(") && !aStrArr.Contains(")"))
            {
                changeSelectClause.Append(" ").Append(aStrArr);
            }
            else
            {
                String[] strArr2 = aStrArr.Split(" ");
                strArr2[strArr2.Length - 1] = "\"" + strArr2[strArr2.Length - 1] + "\"";
                foreach (var aStrArr2 in strArr2)
                {
                    changeSelectClause.Append(" ").Append(aStrArr2);
                }
            }

            changeSelectClause.Append(",");
        }
    }

    public Dictionary<string, object> getParams()
    {
        return parameters;
    }
    
    public SugarParameter[]  getSugarParams()
    {
        return parameters.Select(kv => new SugarParameter(kv.Key, kv.Value)).ToArray();
    }
    

    // public void testParams()
    // {
    //     for (int i = 0; i < parameters.Count; i++)
    //     {
    //         if (i == parameters.Count - 1)
    //         {
    //             Console.WriteLine(parameters[i] + ";");
    //         }
    //         else
    //         {
    //             Console.WriteLine(parameters[i] + ",");
    //         }
    //     }
    // }
    //get and set-----------------------------------------------


    public int getPanum()
    {
        return panum;
    }

    public int getPasiz()
    {
        return pasiz;
    }

    public int getAutoType()
    {
        return autoType;
    }

    public void setAutoType(int autoType)
    {
        this.autoType = autoType;
    }

    public void setPanum(int panum)
    {
        this.panum = panum;
    }

    public void setPasiz(int pasiz)
    {
        this.pasiz = pasiz;
    }
}