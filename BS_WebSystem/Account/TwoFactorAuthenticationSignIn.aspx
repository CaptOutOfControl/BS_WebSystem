<%@ Page Title="双重身份验证" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="TwoFactorAuthenticationSignIn.aspx.cs" Inherits="BS_WebSystem.Account.TwoFactorAuthenticationSignIn" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main aria-labelledby="title">
        <h2 id="title"><%: Title %>。</h2>
        <asp:PlaceHolder runat="server" ID="sendcode">
            <section>
                <h4>发送验证代码</h4>
                <hr />
                <div class="row">
                    <div class="col-md-12">
                        选择双重身份验证提供程序:
                <asp:DropDownList runat="server" ID="Providers">
                </asp:DropDownList>
                        <asp:Button Text="提交" ID="ProviderSubmit" OnClick="ProviderSubmit_Click" CssClass="btn btn-outline-dark" runat="server" />
                    </div>
                </div>
            </section>
        </asp:PlaceHolder>
        <asp:PlaceHolder runat="server" ID="verifycode" Visible="false">
            <section>
                <h4>输入验证代码</h4>
                <hr />
                <asp:HiddenField ID="SelectedProvider" runat="server" />
                <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                    <p class="text-danger">
                        <asp:Literal runat="server" ID="FailureText" />
                    </p>
                </asp:PlaceHolder>
                <div class="row">
                    <asp:Label Text="代码:" runat="server" AssociatedControlID="Code" CssClass="col-md-2 col-form-label" />
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="Code" CssClass="form-control" />
                    </div>
                </div>
                <div class="row">
                    <div class="offset-md-2 col-md-10">
                        <div class="checkbox">
                            <asp:Label Text="记住浏览器" runat="server" />
                            <asp:CheckBox Text="" ID="RememberBrowser" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="offset-md-2 col-md-10">
                        <asp:Button Text="提交" ID="CodeSubmit" OnClick="CodeSubmit_Click" CssClass="btn btn-outline-dark" runat="server" />
                    </div>
                </div>
            </section>
        </asp:PlaceHolder>
    </main>
</asp:Content>
