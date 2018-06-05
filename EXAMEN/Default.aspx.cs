using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.Common.EntitySql;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EXAMEN
{
    public partial class _Default : Page
    {
        private DatabaseEntities _context = new DatabaseEntities();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDropdownControls();
            }
        }

        protected void LoadDropdownControls()
        {
            var companyList = _context.Company.OrderBy(x => x.name).ToList();
            ddlCompanies.DataSource = companyList;
            ddlCompanies.DataValueField = "Id";
            ddlCompanies.DataTextField = "name";
            ddlCompanies.DataBind();
            ddlCompanies.SelectedIndex = 0;

            var Department2List = _context.Company.OrderBy(x => x.name).ToList();
            ddlCompanies2.DataSource = Department2List;
            ddlCompanies2.DataValueField = "Id";
            ddlCompanies2.DataTextField = "name";
            ddlCompanies2.DataBind();
            ddlCompanies2.SelectedIndex = 0;

            ddlCompanyLists.DataSource = companyList;
            ddlCompanyLists.DataValueField = "Id";
            ddlCompanyLists.DataTextField = "name";
            ddlCompanyLists.DataBind();
            ddlCompanyLists.SelectedIndex = 0;

            var jobtitles = _context.JobTitle.OrderBy(x => x.description).ToList();
            ddlEmployeeTitle.DataSource = jobtitles;
            ddlEmployeeTitle.DataValueField = "Id";
            ddlEmployeeTitle.DataTextField = "description";
            ddlEmployeeTitle.DataBind();
            upAddEmployee.Update();

            ddlEditEmployeeJobtitle.DataSource = jobtitles;
            ddlEditEmployeeJobtitle.DataValueField = "Id";
            ddlEditEmployeeJobtitle.DataTextField = "description";
            ddlEditEmployeeJobtitle.DataBind();
            upEditEmployee.Update();

            rpJobtitles.DataSource = jobtitles;
            rpJobtitles.DataBind();
            upJobtitles.Update();
        }

        protected void btnSaveCompany_OnClick(object sender, EventArgs e)
        {
            var companyname = txtCompanyName.Text;
            var companyaddress = txtCompanyAddress.Text;
            var companyheadquarter = txtCompanyHeadquarter.Text;

            var company = new Company();

            company.name = companyname;
            company.address = companyaddress;
            company.headquarter = companyheadquarter;

            _context.Company.Add(company);
            _context.SaveChanges();

            LoadDropdownControls();
            rpDepartments.DataBind();
            upCompanyDetails.Update();
        }

        protected void btnSaveDepartment_OnClick(object sender, EventArgs e)
        {
            var departmentName = txtDepartment.Text;

            var department = new Department();
            department.name = departmentName;

            _context.Department.Add(department);
            _context.SaveChanges();

            var newDepartment = _context.Department.FirstOrDefault(x => x.Id == department.Id);
            var companyDepartment = new CompanyDepartment();

            companyDepartment.fk_company = Convert.ToInt32(ddlCompanies.SelectedValue);
            companyDepartment.fk_department = newDepartment.Id;
            _context.CompanyDepartment.Add(companyDepartment);
            _context.SaveChanges();

            LoadDropdownControls();
            rpDepartments.DataBind();
            upCompanyDetails.Update();
        }

        protected void btnSaveEmployee_OnClick(object sender, EventArgs e)
        {
            var employeeFirstName = txtEmployeeFirstName.Text;
            var employeeLastName = txtEmployeeLastName.Text;
            var employeeBirthday = txtEmployeeBirthday.Text;
            var employeeGender = radioGender.Checked;
            var employeeTitle = ddlEmployeeTitle.SelectedValue;

            var employee = new Employee();

            employee.firstname = employeeFirstName;
            employee.lastname = employeeLastName;
            employee.birthday = Convert.ToDateTime(employeeBirthday);
            employee.gender = employeeGender;
            employee.jobtitle = Convert.ToInt32(employeeTitle);

            _context.Employee.Add(employee);
            _context.SaveChanges();

            var newEmployee = _context.Employee.SingleOrDefault(x =>
                x.firstname == employee.firstname && x.lastname == employee.lastname &&
                employee.birthday == employee.birthday);

            var departmentEmployee = new DepartmenEmployee();
            departmentEmployee.fk_employee = newEmployee.Id;
            departmentEmployee.fk_department = Convert.ToInt32(ddlDepartmens.SelectedValue);
            _context.DepartmenEmployee.Add(departmentEmployee);
            _context.SaveChanges();

            txtEmployeeFirstName.Text = "";
            txtEmployeeLastName.Text = "";
            txtEmployeeBirthday.Text = "";
            radioGender.Checked = false;
            ddlEmployeeTitle.SelectedIndex = 0;

            LoadDropdownControls();
            rpDepartments.DataBind();
            upCompanyDetails.Update();
        }

        protected void ddlCompanies2_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var companyId = Convert.ToInt32(ddlCompanies2.SelectedValue);

            var companydepartments = _context.CompanyDepartment.Where(x => x.Company.Id.Equals(companyId)).ToList().Select(x => new{x.Department.Id, x.Department.name});
            
            ddlDepartmens.DataSource = companydepartments;
            ddlDepartmens.DataValueField = "Id";
            ddlDepartmens.DataTextField = "name";
            ddlDepartmens.DataBind();

            upAddEmployee.Update();
        }

        protected void ddlCompanyLists_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var companyId = Convert.ToInt32(ddlCompanyLists.SelectedValue);

            var companydepartments = _context.CompanyDepartment.Where(x => x.fk_company == companyId).OrderBy(x => x.Company.name).ToList();
            rpDepartments.DataSource = companydepartments;
            rpDepartments.DataBind();
            upCompanyDetails.Update();
        }

        protected void lnkbtnEditDepartment_OnClick(object sender, EventArgs e)
        {
            var btn = (LinkButton)sender;
            Session["departmentid"] = Convert.ToString(int.Parse(btn.CommandArgument));
            var departmentvalue = Convert.ToInt32(Session["departmentid"]);
            var department = _context.Department.FirstOrDefault(x => x.Id == departmentvalue);
            txtEditDepartment.Text = department.name;
            upEditDepartmentText.Update();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$(function() { $('#EditDepartmentModal').modal('show'); });</script>", false);
        }

        protected void btnSaveEditedDepartment_OnClick(object sender, EventArgs e)
        {
            var departmentid = Convert.ToInt32(Session["departmentid"]);
            var department = _context.Department.FirstOrDefault(x => x.Id == departmentid);
            department.name = txtEditDepartment.Text;
            _context.SaveChanges();

            LoadDropdownControls();
            rpDepartments.DataBind();
            upCompanyDetails.Update();
        }

        protected void lnkbtnDeleteDepartment_OnClick(object sender, EventArgs e)
        {
            var btn = (LinkButton)sender;
            var departmentId = int.Parse(btn.CommandArgument);

            //delete foreign key --> employees within department
            var departmentEmployee = _context.DepartmenEmployee.Where(x => x.fk_department == departmentId).ToList();
            if(departmentEmployee != null)
            foreach (var dpe in departmentEmployee)
            {
                _context.DepartmenEmployee.Remove(dpe);
                _context.SaveChanges();
                }
            //delete the department which is linked to a company
            var companyDepartment = _context.CompanyDepartment.FirstOrDefault(x => x.fk_department == departmentId);
            if(companyDepartment != null)
            _context.CompanyDepartment.Remove(companyDepartment);
            _context.SaveChanges();

            //delete actual department
            var department = _context.Department.FirstOrDefault(x => x.Id == departmentId);
            if (department != null)
            {
                _context.Department.Remove(department);
                _context.SaveChanges();
            }

            LoadDropdownControls();
            rpDepartments.DataBind();
            upCompanyDetails.Update();
        }

        protected void rpDepartments_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem != null)
            {
                CompanyDepartment companyDepartment = (CompanyDepartment)e.Item.DataItem;
                var departmentId = companyDepartment.fk_department;

                var departmentEmployees = _context.DepartmenEmployee.Where(x => x.fk_department == departmentId).ToList();
                var rpEmployees = (Repeater)e.Item.FindControl("rpEmployees");
                rpEmployees.DataSource = departmentEmployees;
                rpEmployees.DataBind();
            }
        }

        protected void lnkbtnEditEmployee_OnClick(object sender, EventArgs e)
        {
            var btn = (LinkButton)sender;
            var employeeId = int.Parse(btn.CommandArgument);
            Session["employeeid"] = Convert.ToString(employeeId);

            var employee = _context.Employee.FirstOrDefault(x => x.Id == employeeId);
            if (employee != null)
            {
                txtEditEmployeeFirstName.Text = employee.firstname;
                txtEditEmployeeLastName.Text = employee.lastname;
                txtEditEmployeeBirthday.Text = Convert.ToDateTime(employee.birthday).ToString("yyyy MMMM dd");
                if (employee.gender == true)
                {
                    chkEditEmployeeGender.Checked = true;
                }
                else
                {
                    chkEditEmployeeGender.Checked = false;
                }

                var jobtitle = _context.JobTitle.FirstOrDefault(x => x.Id == employee.jobtitle);
                ddlEditEmployeeJobtitle.SelectedValue = Convert.ToString(jobtitle.Id);
            }
            upEditEmployee.Update();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$(function() { $('#EditEmployeeModal').modal('show'); });</script>", false);
        }

        protected void lnkbtnDeleteEmployee_OnClick(object sender, EventArgs e)
        {
            var btn = (LinkButton)sender;
            var employeeId = int.Parse(btn.CommandArgument);

            var employee = _context.Employee.FirstOrDefault(x => x.Id == employeeId);
            if (employee != null)
            {
                //first delete foreign key
                var departmentEmployee = _context.DepartmenEmployee.FirstOrDefault(x => x.fk_employee == employeeId);
                if (departmentEmployee != null)
                {
                    _context.DepartmenEmployee.Remove(departmentEmployee);
                    _context.SaveChanges();
                }
                //delete actual employee
                _context.Employee.Remove(employee);
                _context.SaveChanges();
            }
            
            LoadDropdownControls();
            rpDepartments.DataBind();
            upCompanyDetails.Update();
        }

        protected void btnSaveEditedEmployee_OnClick(object sender, EventArgs e)
        {
            var employeeId = Convert.ToInt32(Session["employeeid"]);
            var employee = _context.Employee.FirstOrDefault(x => x.Id == employeeId);
            employee.firstname = txtEditEmployeeFirstName.Text;
            employee.lastname = txtEditEmployeeLastName.Text;
            employee.birthday = Convert.ToDateTime(txtEditEmployeeBirthday.Text);
            if (chkEditEmployeeGender.Checked == true)
            {
                employee.gender = true;
            }
            else
            {
                employee.gender = false;
            }
            employee.jobtitle = Convert.ToInt32(ddlEditEmployeeJobtitle.SelectedValue);
            _context.SaveChanges();

            upEditEmployee.Update();

            LoadDropdownControls();
            rpDepartments.DataBind();
            upCompanyDetails.Update();

        }

        protected void btnSaveJobtitle_OnClick(object sender, EventArgs e)
        {
            var txtjobtitle = txtAddJobTitle.Text;
            var jobtitle = new JobTitle();
            jobtitle.description = txtjobtitle;
            _context.JobTitle.Add(jobtitle);
            _context.SaveChanges();
            txtAddJobTitle.Text = "";
            LoadDropdownControls();
        }

        protected void btndeleteJobtitle_OnClick(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            var jobtitleId = int.Parse(btn.CommandArgument);

            var jobtitle = _context.JobTitle.FirstOrDefault(x => x.Id == jobtitleId);
            if (jobtitle != null)
            {
                _context.JobTitle.Remove(jobtitle);
                _context.SaveChanges();
            }
            
            LoadDropdownControls();
        }
    }
}