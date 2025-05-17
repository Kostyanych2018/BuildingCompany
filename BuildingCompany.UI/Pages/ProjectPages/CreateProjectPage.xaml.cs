using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildingCompany.UI.ViewModels.ProjectViewModels;
using Microsoft.Maui.Controls;

namespace BuildingCompany.UI.Pages.ProjectPages;

public partial class CreateProjectPage : ContentPage
{
    public CreateProjectPage(CreateProjectViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}