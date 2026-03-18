<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="True" CodeBehind="Lockout.aspx.cs" Inherits="BS_WebSystem.Account.Lockout" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main>
        <hgroup>
            <h1>已锁定。</h1>
            <h2 class="text-danger">此帐户已锁定，请稍后重试。</h2>
        </hgroup>
    </main>
</asp:Content>
