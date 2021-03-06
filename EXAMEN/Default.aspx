﻿<%@ Page Title="Herxamen ASP.NET 2018" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="EXAMEN._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        function confirmation() {
            if (confirm("Are you sure you want to delete this employee?") == true)
                return true;
            else
                return false;
        }
    </script>
    <div class="container" id="menubuttons">
            <asp:Button ID="btnAddCompany" runat="server" Text="Add company" CssClass="btn btn-info col-xs-2" data-toggle="modal" data-target="#companyModal"/>
            <asp:Button ID="btnAddDepartment" runat="server" Text="Add department" CssClass="btn btn-info col-xs-2" data-toggle="modal" data-target="#departmentModal"/>
            <asp:Button ID="btnAddEmployee" runat="server" Text="Add employee" CssClass="btn btn-info col-xs-2" data-toggle="modal" data-target="#employeeModal"/>
            <asp:Button ID="btnAddJobTitle" runat="server" Text="Add jobtitle" CssClass="btn btn-info col-xs-2" data-toggle="modal" data-target="#jobtitleModal"/>
    </div>
    <div class="container">
        <h2>Choose company:</h2>
        <asp:DropDownList ID="ddlCompanyLists" OnSelectedIndexChanged="ddlCompanyLists_OnSelectedIndexChanged" AutoPostBack="True" CssClass="form-control" runat="server"></asp:DropDownList>
        <asp:UpdatePanel ID="upCompanyDetails" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <h2>Departments: </h2>
                <asp:Repeater ID="rpDepartments" OnItemDataBound="rpDepartments_OnItemDataBound" runat="server">
                    <ItemTemplate>
                    <div class="col-xs-10">
                        <h3><%# Eval("Department.name") %></h3>
                        <asp:LinkButton ID="lnkbtnEditDepartment" CommandArgument='<%# Eval("Department.Id") %>' OnClick="lnkbtnEditDepartment_OnClick" runat="server">Edit</asp:LinkButton>
                        <asp:LinkButton ID="lnkbtnDeleteDepartment" CommandArgument='<%# Eval("Department.Id") %>' OnClick="lnkbtnDeleteDepartment_OnClick" runat="server">Delete</asp:LinkButton>
                    </div>
                        <div class="clearfix"></div>
                        <asp:Repeater ID="rpEmployees" runat="server">
                            <ItemTemplate>
                                <div class="col-xs-10 employees">
                                    <h3><%# Eval("Employee.firstname") %> <%# Eval("Employee.lastname") %></h3>
                                    <asp:LinkButton ID="lnkbtnEditEmployee" CommandArgument='<%# Eval("Employee.Id") %>' OnClick="lnkbtnEditEmployee_OnClick" runat="server">Edit</asp:LinkButton>
                                    <asp:LinkButton ID="lnkbtnDeleteEmployee" CommandArgument='<%# Eval("Employee.Id") %>' OnClientClick="return confirmation();" OnClick="lnkbtnDeleteEmployee_OnClick" runat="server">Delete</asp:LinkButton>
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
                    <h4>Select company</h4>
                    <asp:DropDownList ID="ddlCompanies" runat="server" CssClass="form-control"></asp:DropDownList>
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
                    <asp:UpdatePanel ID="upAddEmployee" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txtEmployeeFirstName" placeholder="employee's first name" runat="server" CssClass="form-control"></asp:TextBox>
                            <asp:TextBox ID="txtEmployeeLastName" placeholder="employee's last name" runat="server" CssClass="form-control"></asp:TextBox>
                            <asp:TextBox ID="txtEmployeeBirthday" placeholder="employee's birthday" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                            <asp:RadioButton ID="radioGender" Text=" Male gender?" runat="server"/>
                            <%--<asp:TextBox ID="txtEmployeeTitle" placeholder="employee's title" runat="server" CssClass="form-control"></asp:TextBox>--%>
                            <h4>Select jobtitle</h4>
                            <asp:DropDownList ID="ddlEmployeeTitle" CssClass="form-control" runat="server"></asp:DropDownList>
                            <h4>Select company</h4>
                            <asp:DropDownList ID="ddlCompanies2" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlCompanies2_OnSelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                            <h4>Select department</h4>
                            <asp:DropDownList ID="ddlDepartmens" runat="server" CssClass="form-control"></asp:DropDownList>
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
    <!-- Modal Jobtitle-->
    <div class="modal fade" id="jobtitleModal" tabindex="-1" role="dialog" aria-labelledby="jobtitleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="jobtitleModalLabel">Add jobtitle</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="upJobtitles" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <asp:Repeater ID="rpJobtitles" runat="server">
                                <ItemTemplate>
                                    <div class="row">
                                        <asp:Label runat="server" CssClass="col-xs-5" Text='<%# Eval("description") %>'></asp:Label>
                                        <asp:Button ID="btndeleteJobtitle" OnClick="btndeleteJobtitle_OnClick" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-danger col-xs-" runat="server" Text="Delete" />
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:TextBox ID="txtAddJobTitle" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                    <asp:Button ID="btnSaveJobtitle" class="btn btn-primary" OnClick="btnSaveJobtitle_OnClick" runat="server" Text="Save jobtitle"/>
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
                            <asp:TextBox ID="txtEditDepartment" CssClass="form-control" runat="server"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                    <asp:Button ID="btnSaveEditedDepartment" class="btn btn-primary" OnClick="btnSaveEditedDepartment_OnClick" runat="server" Text="Save department"/>
                </div>
            </div>
        </div>
    </div>
    <!-- Modal EditEmployeeModal -->
    <div class="modal fade" id="EditEmployeeModal" tabindex="-1" role="dialog" aria-labelledby="EditEmployeeModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="EditEmployeeModalLabel">Edit employee</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="upEditEmployee" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txtEditEmployeeFirstName" placeholder="Voornaam" runat="server" CssClass="form-control"></asp:TextBox>
                            <asp:TextBox ID="txtEditEmployeeLastName" placeholder="Achternaam" runat="server" CssClass="form-control"></asp:TextBox>
                            <asp:TextBox ID="txtEditEmployeeBirthday" TextMode="DateTime" runat="server" CssClass="form-control"></asp:TextBox>
                            <h3>Male gender?</h3>
                            <asp:CheckBox ID="chkEditEmployeeGender" runat="server"/>
                            <%--<asp:TextBox ID="txtEditEmployeeTitle" placeholder="Functie" runat="server" CssClass="form-control"></asp:TextBox>--%>
                            <asp:DropDownList ID="ddlEditEmployeeJobtitle" CssClass="form-control" runat="server"></asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                    <asp:Button ID="btnSaveEditedEmployee" class="btn btn-primary" OnClick="btnSaveEditedEmployee_OnClick" runat="server" Text="Save employee"/>
                </div>
            </div>
        </div>
    </div>
    
</asp:Content>