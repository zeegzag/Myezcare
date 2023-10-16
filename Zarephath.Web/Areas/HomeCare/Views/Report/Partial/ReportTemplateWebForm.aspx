<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportTemplateWebForm.aspx.cs" Inherits="Zarephath.Web.Areas.HomeCare.Views.Report.Partial.ReportTemplateWebForm" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <style>
        @import url('https://fonts.googleapis.com/css?family=Poppins:300,400,500,600,700&display=swap');
    </style>
    <style>
        * {
            box-sizing: border-box;
            margin: 0;
            padding: 0;
            border: 0;
        }
        html {
            height: calc(100% - 6px) !important;
        }
        .height-100 {
            height: 100% !important;
        }
        #ReportModal iframe#frmReport {
            border: none !important;
        }
        #rvSiteMapping {
            height: 100% !important;
            width: 100% !important;
        }
        #rvSiteMapping_ctl08 table tr td label{
            font-family: 'Poppins' !important;
            font-weight:600 !important;
        }
        #VisibleReportContentrvSiteMapping_ctl13 {
        background-color: Transparent;
        border: 1pt none Black;
        padding-left: 100px;
        }
        input, select{
            border:1px solid #d6d6d6;
            height:auto;
            padding:6px 10px;
            font-size:14px;
            max-width:160px;
            margin-bottom: 5px;
        }
        table#ParametersGridrvSiteMapping_ctl08 {
            padding: 12px;
        }
        .page-content iframe#frmReport {
            padding: 0px !important;
            border: none !important;
            height: 360px !important;
        }
        .page-content iframe#frmReport body {
            margin: 0px !important;
        }
        div#rvSiteMapping_ctl08_ctl13 select {
            width: 100%;
            margin-left: 2px;
        }
        .MSRS-RVC .SubmitButtonCell {
            border-left: 1px solid #d6d6d6 !important;
        }
        #rvSiteMapping span.glyphui.glyphui-calendar {
            padding: 0px;
        }
        #VisibleReportContentrvSiteMapping_ctl13{
            padding-left:0px !important;
        }

        table#rvSiteMapping_ctl13{
            border:1px solid #d6d6d6 !important;
        }
        table tr td {
            /*padding: 3px 3px !important;*/
            vertical-align: middle !important;
        }
        #rvSiteMapping_ctl13 table table table tr td {
            /*padding: 3px 2px !important;*/
            font-family: 'Poppins' !important;
        }
        #rvSiteMapping_ctl13 table table tr:nth-child(1) div {
            text-align: center;
            /*font-size: 16px !important;*/
            border-color: #d6d6d6 !important;
        }
        #rvSiteMapping_ctl13 table table tr:nth-child(2) tr td div {
            font-size: 12px !important;
            border-color: #d6d6d6 !important;
            text-align: left;
            font-family: 'Poppins' !important;
        }
        div#rvSiteMapping_ctl13 {
            border: 1px solid #d6d6d6;
            /*background-color:#f8f8f8 !important;*/
        }
        #ParametersRowrvSiteMapping > td {
            padding: 0px !important;
        }
        #Pa715016bc6e14e0fbac97f42af313d02_1_oReportDiv .Abf8b9681da8c4520af57557da116f4cc132 {
            background-color: #e9e9e9 !important;
        }
        #rvSiteMapping .ToolBarButtonsCell{
            text-align:center;
        }
        #rvSiteMapping .ToolbarFind.WidgetSet {
            padding: 7px 10px;
        }
        #rvSiteMapping_ctl13 .Ae04a8b7e94d84465a7ec2d7557442d5a6 .Ae04a8b7e94d84465a7ec2d7557442d5a4 {
            text-align: center !important;
        }
         #rvSiteMapping .ToolBarBackground{
            background-color:#80808029 !important;
        }
        #rvSiteMapping_ReportViewer{
            margin:auto;
        }

        #rvSiteMapping_ctl13{
             align-items: center;
             display: flex;
             justify-content: center;
             height:auto !important;
        }
        #rvSiteMapping_ctl09
        {
           width:auto !important;
        }

</style>
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
</head>
<body class="height-100" style="width:100%;height:auto;display: flex;justify-content:center;">
    <form id="form1" runat="server" class="height-100">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
        <rsweb:ReportViewer ID="rvSiteMapping" Name="PDF" runat="server"
            AsyncRendering="False" BackColor="" ClientIDMode="AutoID" HighlightBackgroundColor="" InternalBorderColor="204, 204, 204" 
            InternalBorderStyle="Solid" InternalBorderWidth="1px" LinkActiveColor="" LinkActiveHoverColor="" 
            LinkDisabledColor="" PrimaryButtonBackgroundColor="" PrimaryButtonForegroundColor="" PrimaryButtonHoverBackgroundColor="" 
            PrimaryButtonHoverForegroundColor="" ProcessingMode="Remote" SecondaryButtonBackgroundColor="" SecondaryButtonForegroundColor="" 
            SecondaryButtonHoverBackgroundColor="" SecondaryButtonHoverForegroundColor="" SplitterBackColor="" 
            ToolbarDividerColor="" ToolbarForegroundColor="" ToolbarForegroundDisabledColor="" 
            ToolbarHoverBackgroundColor="" ToolbarHoverForegroundColor="" ToolBarItemBorderColor="" ToolBarItemBorderStyle="Solid" 
            ToolBarItemBorderWidth="1px" ToolBarItemHoverBackColor="" ToolBarItemPressedBorderColor="51, 102, 153" 
            ToolBarItemPressedBorderStyle="Solid" Width="100%" ToolBarItemPressedBorderWidth="1px" ToolBarItemPressedHoverBackColor="153, 187, 226" SizeToReportContent="False" >
        </rsweb:ReportViewer>
    </form>
    <%--  <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <rsweb:ReportViewer ID="rvSiteMapping" runat="server" Width="100%" Height="100%" ShowParameterPrompts="true" AsyncRendering="False" ShowReportBody="true">
        </rsweb:ReportViewer>
        </form>--%>


    <table>
        <tr>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
        </tr>
    </table>





</body>
   
</html>

<script src="../../../../../Assets/js/viewjs/siteApp/layout.js"></script>

