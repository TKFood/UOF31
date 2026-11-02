using Ede.Uof.Utility.Page.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;

public partial class CDS_KYTUtils_QUERYWINDOWS_UC_SelectUserFilterGroup : System.Web.UI.UserControl
{
    private string m_GroupID;
    public string GroupID
    {
        get { return hidGroup.Value; }
        set
        {
            hidGroup.Value = value;
            SetDialog();
        }
    }

    /// <summary>
    /// 開窗標題
    /// </summary>
    public string Title
    {
        get { return hidTitle.Value; }
        set
        {
            hidTitle.Value = value;
            SetDialog();
        }
    }

    /// <summary>
    /// 開窗寬度
    /// </summary>
    public int WindowWidth
    {
        get { return int.Parse(hidWidth.Value); }
        set
        {
            hidWidth.Value = value.ToString();
            SetDialog();
        }
    }

    /// <summary>
    /// 開窗高度
    /// </summary>
    public int WindowHeight
    {
        get { return int.Parse(hidHeight.Value); }
        set
        {
            hidHeight.Value = value.ToString();
            SetDialog();
        }
    }

    /// <summary>
    /// 按鈕文字
    /// </summary>
    public string ButtonText
    {
        get { return hidButtonText.Value; }
        set
        {
            hidButtonText.Value = value;
            SetDialog();
        }
    }

    /// <summary>
    /// 從部門選擇人員 -> 否，選擇人員元件
    /// </summary>
    public bool SelectWithGroup
    {
        get { return hidIsGroup.Value == "Y"; }
        set
        {
            hidIsGroup.Value = value ? "Y" : "N";
            SetDialog();
        }
    }

    /// <summary>
    /// 單選?(如果SelectWithGroup為True則不生效)
    /// </summary>
    public bool SingleSelect
    {
        get { return hidIsSingle.Value == "Y"; }
        set
        {
            hidIsSingle.Value = value ? "Y" : "N";
            SetDialog();
        }
    }

    /// <summary>
    /// 部門篩選的ConnectionString
    /// </summary>
    public string GroupConnectionString
    {
        get { return hidGroupCS.Value; }
        set
        {
            hidGroupCS.Value = value;
            SetDialog();
        }
    }

    /// <summary>
    /// 部門篩選的Sql指令(Ex.SELECT * FROM[TB_EB_GROUP])
    /// </summary>
    public string GroupSql
    {
        get { return hidGroupSQL.Value; }
        set
        {
            hidGroupSQL.Value = value;
            SetDialog();
        }
    }

    /// <summary>
    /// 選擇部門後篩選使用者的ConnectionString
    /// </summary>
    public string UserConnectionString
    {
        get { return hidUserCS.Value; }
        set
        {
            hidUserCS.Value = value;
            SetDialog();
        }
    }

    /// <summary>
    /// 選擇部門後篩選使用者的Sql指令(Ex.SELECT * FROM[TB_EB_EMPL_DEP] WHERE[GROUP_ID] = @GROUPID)
    /// </summary>
    public string UserSql
    {
        get { return hidUserSQL.Value; }
        set
        {
            hidUserSQL.Value = value;
            SetDialog();
        }
    }

    /// <summary>
    /// 搜尋SQL指令(Ex.SELECT * FROM[TB_EB_EMPL_DEP] WHERE GROUP_ID IN({0}) AND ACCOUNT LIKE '%' + @SEARCHTEXT + '%'
    /// </summary>
    public string SearchSql
    {
        get { return hidSearchSQL.Value; }
        set
        {
            hidSearchSQL.Value = value;
            SetDialog();
        }
    }

    /// <summary>
    /// 先前所選擇的人員
    /// </summary>
    public string SelectedUsers
    {
        get { return hidSelectedUser.Value; }
        set
        {
            hidSelectedUser.Value = value;
            SetDialog();
        }
    }

    /// <summary>
    /// 是否為手機使用
    /// </summary>
    public bool isMobile
    {
        get { return this.WindowWidth <= 375; }
        set
        {
            this.WindowWidth = value ? 375 : this.WindowWidth;
            SetDialog();
        }
    }

    /// <summary>
    /// 是否查詢自己的部門人員
    /// </summary>
    public bool isSearchSelfGroupUsers
    {
        get { return hidIsSearchSelfGroupUsers.Value == "Y"; }
        set
        {
            hidIsSearchSelfGroupUsers.Value = value ? "Y" : "N";
            SetDialog();
        }
    }

    /// <summary>
    /// OnDialogReturn的Delegate
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="result"></param>
    public delegate void DelegateOnDialogReturn(object sender, string result);

    /// <summary>
    /// OnDialogReturn事件
    /// </summary>
    public event DelegateOnDialogReturn DialogReturn;

    /// <summary>
    /// 建構單元
    /// </summary>
    public CDS_KYTUtils_QUERYWINDOWS_UC_SelectUserFilterGroup()
    {
    }

    /// <summary>
    /// Page_Load
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(hidButtonText.Value))
            this.Button1.Text = hidButtonText.Value;
        SetDialog();
    }

    /// <summary>
    /// 設定開窗
    /// </summary>
    private void SetDialog()
    {
        // 設定Dialog Open
        Dialog.Open2(
            this.Button1,
            "~/CDS/KYTUtils/QUERYWINDOWS/SelectUserFilterGroup.aspx",
            hidTitle.Value,
            int.Parse(hidWidth.Value),
            int.Parse(hidHeight.Value),
            Dialog.PostBackType.AfterReturn,
            new
            {
                GROUPID = HttpUtility.UrlEncode(hidGroup.Value),
                selectedUsers = HttpUtility.UrlEncode(this.SelectedUsers),
                isSingle = HttpUtility.UrlEncode(hidIsSingle.Value),
                isGroup = HttpUtility.UrlEncode(hidIsGroup.Value),
                isSearchSelfGroupUsers = HttpUtility.UrlEncode(hidIsSearchSelfGroupUsers.Value),
                GroupConnectionString = HttpUtility.UrlEncode(this.hidGroupCS.Value),
                GroupSql = HttpUtility.UrlEncode(this.hidGroupSQL.Value),
                UserConnectionString = HttpUtility.UrlEncode(this.hidUserCS.Value),
                UserSql = HttpUtility.UrlEncode(this.hidUserSQL.Value),
                SearchSql = HttpUtility.UrlEncode(this.hidSearchSQL.Value)
            }.ToExpando());

    }

    /// <summary>
    /// 開窗結果回傳事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button1_Click(object sender, EventArgs e)
    {
        string resultValue = Dialog.GetReturnValue();
        DataTable table = JsonConvert.DeserializeObject<DataTable>(resultValue);

        Dictionary<string, List<string>> result = new Dictionary<string, List<string>>();
        foreach (DataRow dr in table.Rows)
        {
            string GID = dr["GROUP_ID"].ToString();
            if (!result.ContainsKey(GID)) result.Add(GID, new List<string>());
            result[GID].Add(dr["GUID"].ToString());
        }
        JArray jArray = new JArray();
        foreach (KeyValuePair<string, List<string>> item in result)
        {
            JObject jObj = new JObject();
            jObj.Add(new JProperty("Group", item.Key));
            jObj.Add(new JProperty("Users", item.Value));
            jArray.Add(jObj);
        }
        this.SelectedUsers = JsonConvert.SerializeObject(jArray);
        DialogReturn.Invoke(sender, resultValue);
    }
}