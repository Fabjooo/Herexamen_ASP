﻿<%@ Page Title="Herxamen ASP.NET 2018" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="EXAMEN._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="container" id="menubuttons">
            <asp:Button ID="btnAddCompany" runat="server" Text="Add company" CssClass="btn btn-info col-xs-2" data-toggle="modal" data-target="#companyModal"/>
            <asp:Button ID="btnAddDepartment" runat="server" Text="Add department" CssClass="btn btn-info col-xs-2" data-toggle="modal" data-target="#departmentModal"/>
            <asp:Button ID="btnAddEmployee" runat="server" Text="Add employee" CssClass="btn btn-info col-xs-2" data-toggle="modal" data-target="#employeeModal"/>
    </div>
    <div class="container">
        <asp:DropDownList ID="ddlCompanyLists" OnSelectedIndexChanged="ddlCompanyLists_OnSelectedIndexChanged" AutoPostBack="True" runat="server"></asp:DropDownList>
        <asp:UpdatePanel ID="upCompanyDetails" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <asp:Repeater ID="rpCompany" OnItemDataBound="rpCompany_OnItemDataBound" runat="server">
                    <ItemTemplate>
                    <div class="row">
                        <h3><%# Eval("Department.name") %></h3>
                        <asp:LinkButton ID="lnkbtnEdit" CommandArgument='<%# Eval("Department.Id") %>' OnClick="lnkbtnEdit_OnClick" runat="server">Edit</asp:LinkButton>
                        <asp:LinkButton ID="lnkbtnDelete" CommandArgument='<%# Eval("Department.Id") %>' OnClick="lnkbtnDelete_OnClick" runat="server">Delete</asp:LinkButton>
                    </div>
                        <asp:Repeater ID="rpEmployees" runat="server">
                            <ItemTemplate>
                                <div class="row">
                                    <h4><%# Eval("Employee.firstname") %> <%# Eval("Employee.lastname") %></h4>

                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ItemTemplate>
                </asp:Repeater>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    
    <!-- Modal Company-->
    <div class="modal fade" id="companyModal" tabindex="-1" role="dialog" aria-labelledby="companyModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel">Add company</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <asp:TextBox ID="txtCompanyName" placeholder="company name" runat="server" CssClass="form-control"></asp:TextBox>
                    <asp:TextBox ID="txtCompanyAddress" placeholder="company address" runat="server" CssClass="form-control"></asp:TextBox>
                    <asp:TextBox ID="txtCompanyHeadquarter" placeholder="company headquarter" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                    <asp:Button ID="btnSaveCompany" class="btn btn-primary" OnClick="btnSaveCompany_OnClick" runat="server" Text="Save company"/>
                </div>
            </div>
        </div>
    </div>
    <!-- Modal Department-->
    <div class="modal fade" id="departmentModal" tabindex="-1" role="dialog" aria-labelledby="departmentModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="departmentModalLabel">Add department</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <asp:TextBox ID="txtDepartment" placeholder="department name" runat="server" CssClass="form-control"></asp:TextBox>
                    <asp:DropDownList ID="ddlCompanies" runat="server"></asp:DropDownList>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                    <asp:Button ID="btnSaveDepartment" class="btn btn-primary" OnClick="btnSaveDepartment_OnClick" runat="server" Text="Save department"/>
                </div>
            </div>
        </div>
    </div>
    <!-- Modal Employee-->
    <div class="modal fade" id="employeeModal" tabindex="-1" role="dialog" aria-labelledby="employeeModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="employeeModalLabel">Add employee</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txtEmployeeFirstName" placeholder="employee's first name" runat="server" CssClass="form-control"></asp:TextBox>
                            <asp:TextBox ID="txtEmployeeLastName" placeholder="employee's last name" runat="server" CssClass="form-control"></asp:TextBox>
                            <asp:TextBox ID="txtEmployeeBirthday" placeholder="employee's birthday" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                            <asp:RadioButton ID="radioGender" Text=" Male gender?" runat="server"/>
                            <asp:TextBox ID="txtEmployeeTitle" placeholder="employee's title" runat="server" CssClass="form-control"></asp:TextBox>

                            <asp:DropDownList ID="ddlCompanies2" runat="server" OnSelectedIndexChanged="ddlCompanies2_OnSelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                            <asp:DropDownList ID="ddlDepartmens" runat="server"></asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                    <asp:Button ID="btnSaveEmployee" class="btn btn-primary" OnClick="btnSaveEmployee_OnClick" runat="server" Text="Save employee"/>
                </div>
            </div>
        </div>
    </div>
    <!-- Modal EditDepartment -->
    <div class="modal fade" id="EditDepartmentModal" tabindex="-1" role="dialog" aria-labelledby="EditDepartmentModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="EditDepartmentModalLabel">Edit department</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="upEditDepartmentText" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txtEditDepartment" runat="server"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                    <asp:Button ID="btnSaveEditedDepartment" class="btn btn-primary" OnClick="btnSaveEditedDepartment_OnClick" CommandArgument='<%# Eval("Department.Id") %>' runat="server" Text="Save department"/>
                </div>
            </div>
        </div>
    </div>
</asp:Content>