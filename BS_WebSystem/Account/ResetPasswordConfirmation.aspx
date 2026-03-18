<%@ Page Title="密码已更改" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ResetPasswordConfirmation.aspx.cs" Inherits="BS_WebSystem.Account.ResetPasswordConfirmation" Async="true" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <main aria-labelledby="title">
        <h2 id="title"><%: Title %>.</h2>
        <div>
            <p>已更改你的密码。单击 <asp:HyperLink ID="login" runat="server" NavigateUrl="~/Account/Login">此处</asp:HyperLink> 登录 </p>
        </div>
    </main>
</asp:Content>
