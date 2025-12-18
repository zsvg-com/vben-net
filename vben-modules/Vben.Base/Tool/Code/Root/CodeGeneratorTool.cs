using JinianNet.JNTemplate;

namespace Vben.Base.Tool.Code.Root;

/// <summary>
/// 代码生成器
/// </summary>
public class CodeGeneratorTool
{
    /// <summary>
    /// 代码生成器配置
    /// </summary>
    private static CodeGenerateOption _option = new CodeGenerateOption();

    /// <summary>
    /// 代码生成器入口方法
    /// </summary>
    /// <param name="dto"></param>
    public static void Generate(GenerateDto dto)
    {
        string[] arr = dto.GenTable.name.Split("_");
        _option.BaseNamespace = "Vben.Admin";
        _option.Namespace = _option.BaseNamespace + "." + XstrUtil.FirstCharToUpper(arr[0]);
        


        // _option.SubNamespace = "xxx";
        // _option.DtosNamespace = _option.BaseNamespace + "Model";
        // _option.ModelsNamespace = _option.BaseNamespace + "Model";
        // _option.RepositoriesNamespace = _option.BaseNamespace + "Repository";
        // _option.IRepositoriesNamespace = _option.BaseNamespace + "Repository";
        // _option.IServicsNamespace = _option.BaseNamespace + "Service";
        // _option.ServicesNamespace = _option.BaseNamespace + "Service";
        // _option.ApiControllerNamespace = _option.BaseNamespace + "Admin.WebApi";

        // var vuePath = AppSettings.GetConfig("gen:vuePath");
        // var vuePath = "";
        // dto.VueParentPath = dto.VueVersion == 3 ? "ZRAdmin-vue" : "ZR.Vue";
        // if (!vuePath.IsEmpty())
        // {
        //    
        // }
        // dto.VueParentPath = vuePath;
        dto.GenOptions = _option;

        // string PKName = "Id";
        // string PKType = "int";
        ReplaceDto replaceDto = new();
        replaceDto.UrlName = dto.GenTable.name.Replace("_", "/"); //表名对应C# 实体类名
        // replaceDto.ModelTypeName = "xxx"; //表名对应C# 实体类名
        // replaceDto.PermissionPrefix = "xxx";
        // replaceDto.Author = "xxx";
        // replaceDto.ShowBtnAdd = dto.GenTable.Options.CheckedBtn.Any(f => f == 1);
        // replaceDto.ShowBtnEdit = dto.GenTable.Options.CheckedBtn.Any(f => f == 2);
        // replaceDto.ShowBtnDelete = dto.GenTable.Options.CheckedBtn.Any(f => f == 3);
        // replaceDto.ShowBtnExport = dto.GenTable.Options.CheckedBtn.Any(f => f == 4);
        // replaceDto.ShowBtnView = dto.GenTable.Options.CheckedBtn.Any(f => f == 5);


        //循环表字段信息
        // foreach (GenTableColumn dbFieldInfo in dto.GenTable.Columns.OrderBy(x => x.Sort))
        // {
        //     if (dbFieldInfo.IsPk || dbFieldInfo.IsIncrement)
        //     {
        //         PKName = dbFieldInfo.CsharpField;
        //         PKType = dbFieldInfo.CsharpType;
        //     }
        //     if (dbFieldInfo.HtmlType.Equals(GenConstants.HTML_IMAGE_UPLOAD) || dbFieldInfo.HtmlType.Equals(GenConstants.HTML_FILE_UPLOAD))
        //     {
        //         replaceDto.UploadFile = 1;
        //     }
        //     dbFieldInfo.CsharpFieldFl = dbFieldInfo.CsharpField.FirstLowerCase();
        // }

        // replaceDto.PKName = PKName;
        // replaceDto.PKType = PKType;
        // replaceDto.FistLowerPk = "xxx";
        InitJntTemplate(dto, replaceDto);

        GenerateModels(replaceDto, dto);
        // GenerateRepository(replaceDto, dto);
        GenerateService(replaceDto, dto);
        GenerateControllers(replaceDto, dto);
        
        GenerateIndexVue(replaceDto, dto);
        GenerateEditVue(replaceDto, dto);

        if (dto.GenTable.rotyp == "front")
        {
            GenerateRouteJson(replaceDto, dto);
        }else if (dto.GenTable.rotyp == "back")
        {
            GenerateRouteSql(replaceDto, dto);
        }
   
     
        // if (dto.VueVersion == 3)
        // {
        //     GenerateVue3Views(replaceDto, dto);
        // }
        // else
        // {
        //     replaceDto.VueViewListHtml = GenerateVueTableList();
        //     replaceDto.VueQueryFormHtml = GenerateVueQueryForm();
        //     replaceDto.VueViewFormHtml = GenerateCurdForm();
        //
        //     GenerateVueViews(replaceDto, dto);
        // }
        // GenerateVueJs(replaceDto, dto);
        // GenerateSql(replaceDto, dto);

        if (dto.IsPreview) return;

        foreach (var item in dto.GenCodes)
        {
            item.Path = Path.Combine(dto.GenCodePath, item.Path);
            FileHelper.WriteAndSave(item.Path, item.Content);
        }
    }

    #region 读取模板

    /// <summary>
    /// 生成实体类Model
    /// </summary>
    private static void GenerateModels(ReplaceDto replaceDto, GenerateDto generateDto)
    {
        var tpl = FileHelper.ReadJtTemplate("C#","entity.vm");
        // var tplDto = FileHelper.ReadJtTemplate("TplDto.txt");
        string[] arr = generateDto.GenTable.name.Split("_");
        string fullPath = "";
        if (arr.Length == 1)
        {
            fullPath = Path.Combine("C#", XstrUtil.FirstCharToUpper(arr[0]), generateDto.GenTable.bunam + ".cs");
        }
        else if (arr.Length == 2)
        {
            fullPath = Path.Combine("C#", XstrUtil.FirstCharToUpper(arr[0]), XstrUtil.FirstCharToUpper(arr[1]),
                generateDto.GenTable.bunam + ".cs");
        }
        else if (arr.Length == 3)
        {
            fullPath = Path.Combine("C#", XstrUtil.FirstCharToUpper(arr[0]), XstrUtil.FirstCharToUpper(arr[1]),
                XstrUtil.FirstCharToUpper(arr[2]), generateDto.GenTable.bunam + ".cs");
        }
        else if (arr.Length == 4)
        {
            fullPath = Path.Combine("C#", XstrUtil.FirstCharToUpper(arr[0]), XstrUtil.FirstCharToUpper(arr[1]),
                XstrUtil.FirstCharToUpper(arr[2]), XstrUtil.FirstCharToUpper(arr[3]),
                generateDto.GenTable.bunam + ".cs");
        }

        // string fullPathDto = Path.Combine(_option.ModelsNamespace, "Dto", _option.SubNamespace, $"{replaceDto.ModelTypeName}Dto.cs");
        generateDto.GenCodes.Add(new GenCode(1, "Entity.cs", fullPath, tpl.Render()));
        // generateDto.GenCodes.Add(new GenCode(2, "Dto.cs", fullPathDto, tplDto.Render()));
    }

    // /// <summary>
    // /// 生成Repository层代码文件
    // /// </summary>
    // /// <param name="generateDto"></param>
    // /// <param name="replaceDto">替换实体</param>
    // private static void GenerateRepository(ReplaceDto replaceDto, GenerateDto generateDto)
    // {
    //     var tpl = FileHelper.ReadJtTemplate("C#","TplRepository.txt");
    //     var result = tpl.Render();
    //     var fullPath = Path.Combine(_option.RepositoriesNamespace, _option.SubNamespace,
    //         $"{replaceDto.ModelTypeName}Repository.cs");
    //
    //     generateDto.GenCodes.Add(new GenCode(3, "Repository.cs", fullPath, result));
    // }

    /// <summary>
    /// 生成Service文件
    /// </summary>
    private static void GenerateService(ReplaceDto replaceDto, GenerateDto generateDto)
    {
        var tpl = FileHelper.ReadJtTemplate("C#","service.vm");
        var result = tpl.Render();

        string[] arr = generateDto.GenTable.name.Split("_");
        string fullPath = "";
        if (arr.Length == 1)
        {
            fullPath = Path.Combine("C#", XstrUtil.FirstCharToUpper(arr[0]),
                generateDto.GenTable.bunam + "Service.cs");
        }
        else if (arr.Length == 2)
        {
            fullPath = Path.Combine("C#", XstrUtil.FirstCharToUpper(arr[0]), XstrUtil.FirstCharToUpper(arr[1]),
                generateDto.GenTable.bunam + "Service.cs");
        }
        else if (arr.Length == 3)
        {
            fullPath = Path.Combine("C#", XstrUtil.FirstCharToUpper(arr[0]), XstrUtil.FirstCharToUpper(arr[1]),
                XstrUtil.FirstCharToUpper(arr[2]),
                generateDto.GenTable.bunam + "Service.cs");
        }
        else if (arr.Length == 4)
        {
            fullPath = Path.Combine("C#", XstrUtil.FirstCharToUpper(arr[0]), XstrUtil.FirstCharToUpper(arr[1]),
                XstrUtil.FirstCharToUpper(arr[2]), XstrUtil.FirstCharToUpper(arr[3]),
                generateDto.GenTable.bunam + "Service.cs");
        }

        // var fullPath = Path.Combine(_option.ServicesNamespace, _option.SubNamespace,
        //     $"{replaceDto.ModelTypeName}Service.cs");

        generateDto.GenCodes.Add(new GenCode(4, "Service.cs", fullPath, result));
    }

    /// <summary>
    /// 生成控制器ApiControllers文件
    /// </summary>
    private static void GenerateControllers(ReplaceDto replaceDto, GenerateDto generateDto)
    {
        var tpl = FileHelper.ReadJtTemplate("C#","controller.vm");
        // tpl.Set("QueryCondition", replaceDto.QueryCondition);
        var result = tpl.Render();

        // var fullPath = Path.Combine(_option.ApiControllerNamespace, "Controllers", _option.SubNamespace,
        //     $"{replaceDto.ModelTypeName}Controller.cs");
        string[] arr = generateDto.GenTable.name.Split("_");
        string fullPath = "";
        if (arr.Length == 1)
        {
            fullPath = Path.Combine("C#", XstrUtil.FirstCharToUpper(arr[0]), generateDto.GenTable.bunam + "Api.cs");
        }
        else if (arr.Length == 2)
        {
            fullPath = Path.Combine("C#", XstrUtil.FirstCharToUpper(arr[0]), XstrUtil.FirstCharToUpper(arr[1]),
                generateDto.GenTable.bunam + "Api.cs");
        }
        else if (arr.Length == 3)
        {
            fullPath = Path.Combine("C#", XstrUtil.FirstCharToUpper(arr[0]), XstrUtil.FirstCharToUpper(arr[1]),
                XstrUtil.FirstCharToUpper(arr[2]),
                generateDto.GenTable.bunam + "Api.cs");
        }
        else if (arr.Length == 4)
        {
            fullPath = Path.Combine("C#", XstrUtil.FirstCharToUpper(arr[0]), XstrUtil.FirstCharToUpper(arr[1]),
                XstrUtil.FirstCharToUpper(arr[2]), XstrUtil.FirstCharToUpper(arr[3]),
                generateDto.GenTable.bunam + "Api.cs");
        }

        generateDto.GenCodes.Add(new GenCode(5, "Api.cs", fullPath, result));
    }

    /// <summary>
    /// 生成前段index.vue文件
    /// </summary>
    private static void GenerateIndexVue(ReplaceDto replaceDto, GenerateDto generateDto)
    {
        ITemplate tpl;
        if (generateDto.GenTable.edtyp == "modal")
        {
            tpl = FileHelper.ReadJtTemplate("Web","modalIndex.vue.vm");
        }
        else if(generateDto.GenTable.edtyp == "drawer")
        {
            tpl = FileHelper.ReadJtTemplate("Web","drawerIndex.vue.vm");
        }
        else
        {
            tpl = FileHelper.ReadJtTemplate("Web","tabIndex.vue.vm");
        }
        
        // tpl.Set("QueryCondition", replaceDto.QueryCondition);
        var result = tpl.Render();

        // var fullPath = Path.Combine(_option.ApiControllerNamespace, "Controllers", _option.SubNamespace,
        //     $"{replaceDto.ModelTypeName}Controller.cs");
        string[] arr = generateDto.GenTable.name.Split("_");
        string fullPath = "";
        if (arr.Length == 1)
        {
            fullPath = Path.Combine("Web", arr[0], "index.vue");
        }
        else if (arr.Length == 2)
        {
            fullPath = Path.Combine("Web",arr[0], arr[1],"index.vue");
        }
        else if (arr.Length == 3)
        {
            fullPath = Path.Combine("Web", arr[0], arr[1],arr[2], "index.vue");
        }
        else if (arr.Length == 4)
        {
            fullPath = Path.Combine("Web", arr[0], arr[1],arr[2], arr[3], "index.vue");
        }

        generateDto.GenCodes.Add(new GenCode(5, "index.vue", fullPath, result));
    }

    /// <summary>
    /// 生成前端edit.vue文件
    /// </summary>
    private static void GenerateEditVue(ReplaceDto replaceDto, GenerateDto generateDto)
    {
        
        ITemplate tpl;
        if (generateDto.GenTable.edtyp == "modal")
        {
            tpl = FileHelper.ReadJtTemplate("Web","modalEdit.vue.vm");
        }
        else if(generateDto.GenTable.edtyp == "drawer")
        {
            tpl = FileHelper.ReadJtTemplate("Web","drawerEdit.vue.vm");
        }
        else
        {
            tpl = FileHelper.ReadJtTemplate("Web","tabEdit.vue.vm");
        }
        
        // tpl.Set("QueryCondition", replaceDto.QueryCondition);
        var result = tpl.Render();

        // var fullPath = Path.Combine(_option.ApiControllerNamespace, "Controllers", _option.SubNamespace,
        //     $"{replaceDto.ModelTypeName}Controller.cs");
        string[] arr = generateDto.GenTable.name.Split("_");
        string fullPath = "";
        if (arr.Length == 1)
        {
            fullPath = Path.Combine("Web", arr[0], "edit.vue");
        }
        else if (arr.Length == 2)
        {
            fullPath = Path.Combine("Web", arr[0], arr[1],"edit.vue");
        }
        else if (arr.Length == 3)
        {
            fullPath = Path.Combine("Web", arr[0], arr[1],arr[2], "edit.vue");
        }
        else if (arr.Length == 4)
        {
            fullPath = Path.Combine("Web", arr[0], arr[1],arr[2], arr[3], "edit.vue");
        }

        generateDto.GenCodes.Add(new GenCode(5, "edit.vue", fullPath, result));
    }
    
    /// <summary>
    /// 生成前端路由json文件
    /// </summary>
    private static void GenerateRouteJson(ReplaceDto replaceDto, GenerateDto generateDto)
    {
        var tpl = FileHelper.ReadJtTemplate("Route","route.json.vm");
        // tpl.Set("QueryCondition", replaceDto.QueryCondition);
        var result = tpl.Render();

        // var fullPath = Path.Combine(_option.ApiControllerNamespace, "Controllers", _option.SubNamespace,
        //     $"{replaceDto.ModelTypeName}Controller.cs");
        string[] arr = generateDto.GenTable.name.Split("_");
        string fullPath = "";
        if (arr.Length == 1)
        {
            fullPath = Path.Combine("Route", arr[0], "route.json");
        }
        else if (arr.Length == 2)
        {
            fullPath = Path.Combine("Route", arr[0], arr[1],"route.json");
        }
        else if (arr.Length == 3)
        {
            fullPath = Path.Combine("Route", arr[0], arr[1],arr[2], "route.json");
        }
        else if (arr.Length == 4)
        {
            fullPath = Path.Combine("Route", arr[0], arr[1],arr[2], arr[3], "route.json");
        }

        generateDto.GenCodes.Add(new GenCode(5, "route.json", fullPath, result));
    }

    /// <summary>
    /// 生成后端路由sql文件
    /// </summary>
    private static void GenerateRouteSql(ReplaceDto replaceDto, GenerateDto generateDto)
    {
        var tpl = FileHelper.ReadJtTemplate("Route","route.sql.vm");
        // tpl.Set("QueryCondition", replaceDto.QueryCondition);
        var result = tpl.Render();

        // var fullPath = Path.Combine(_option.ApiControllerNamespace, "Controllers", _option.SubNamespace,
        //     $"{replaceDto.ModelTypeName}Controller.cs");
        string[] arr = generateDto.GenTable.name.Split("_");
        string fullPath = "";
        if (arr.Length == 1)
        {
            fullPath = Path.Combine("Route", arr[0], "route.sql");
        }
        else if (arr.Length == 2)
        {
            fullPath = Path.Combine("Route", arr[0], arr[1],"route.sql");
        }
        else if (arr.Length == 3)
        {
            fullPath = Path.Combine("Route", arr[0], arr[1],arr[2], "route.sql");
        }
        else if (arr.Length == 4)
        {
            fullPath = Path.Combine("Route", arr[0], arr[1],arr[2], arr[3], "route.sql");
        }

        generateDto.GenCodes.Add(new GenCode(5, "route.sql", fullPath, result));
    }

    /// <summary>
    /// 获取前端标签名
    /// </summary>
    /// <param name="columnDescription"></param>
    /// <param name="columnName"></param>
    /// <returns></returns>
    public static string GetLabelName(string columnDescription, string columnName)
    {
        return string.IsNullOrEmpty(columnDescription) ? columnName : columnDescription;
    }

    /// <summary>
    /// 首字母转小写,模板使用(勿删)
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string FirstLowerCase(string str)
    {
        try
        {
            return string.IsNullOrEmpty(str) ? str : str.Substring(0, 1).ToLower() + str[1..];
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return "";
        }
    }

    /// <summary>
    /// 获取C# 类型
    /// </summary>
    /// <param name="sDatatype"></param>
    /// <returns></returns>
    public static string GetCSharpDatatype(string sDatatype)
    {
        sDatatype = sDatatype.ToLower();
        string sTempDatatype = sDatatype switch
        {
            "int" or "number" or "integer" or "smallint" => "int",
            "bigint" => "long",
            "tinyint" => "byte",
            "numeric" or "real" or "float" => "float",
            "decimal" or "numer(8,2)" or "numeric" => "decimal",
            "bit" => "bool",
            "date" or "datetime" or "datetime2" or "smalldatetime" or "timestamp" => "DateTime",
            "money" or "smallmoney" => "decimal",
            _ => "string",
        };
        return sTempDatatype;
    }

    public static bool IsNumber(string tableDataType)
    {
        string[] arr = new string[] {"int", "long"};
        return arr.Any(f => f.Contains(GetCSharpDatatype(tableDataType)));
    }

    #endregion


    /// <summary>
    /// 初始化Jnt模板
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="replaceDto"></param>
    private static void InitJntTemplate(GenerateDto dto, ReplaceDto replaceDto)
    {
        Engine.Current.Clean();
        dto.GenTable.fields = dto.GenTable.fields.OrderBy(x => x.ornum).ToList();
        // bool showCustomInput = dto.GenTable.fields.Any(f => f.HtmlType.Equals(GenConstants.HTML_CUSTOM_INPUT, StringComparison.OrdinalIgnoreCase));
        //jnt模板引擎全局变量
        Engine.Configure((options) =>
        {
            options.TagPrefix = "${";
            options.TagSuffix = "}";
            options.TagFlag = '$';
            options.OutMode = OutMode.Auto;
            //options.DisableeLogogram = true;//禁用简写
            options.Data.Set("refs", "$"); //特殊标签替换
            options.Data.Set("t", "$"); //特殊标签替换
            options.Data.Set("modal", "$"); //特殊标签替换
            options.Data.Set("index", "$"); //特殊标签替换
            options.Data.Set("confirm", "$"); //特殊标签替换
            options.Data.Set("nextTick", "$");
            options.Data.Set("replaceDto", replaceDto);
            options.Data.Set("options", dto.GenOptions);
            options.Data.Set("genTable", dto.GenTable);
            //options.Data.Set("btns", dto.CheckedBtn);
            // options.Data.Set("showCustomInput", showCustomInput);
            options.Data.Set("tool", new CodeGeneratorTool());
            options.Data.Set("codeTool", new CodeGenerateTemplate());
            options.EnableCache = true;
            //...其它数据
        });
    }
}