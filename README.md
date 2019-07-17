[![KGy SOFT .net](https://docs.kgysoft.net/corelibraries/icons/logo.png)](https://kgysoft.net)

# KGySoft.ComponentModelDemo
This repo is a demo WPF/WinForms application that demonstrates some features of the [KGySoft.ComponentModel](http://docs.kgysoft.net/corelibraries/?topic=html/N_KGySoft_ComponentModel.htm) namespace of [KGy SOFT Core Libraries](https://kgysoft.net/corelibraries) and also provides some useful solutions for using the KGy SOFT Core Libraries in WPF and Windows Forms applications.

[![Website](https://img.shields.io/website/https/kgysoft.net/corelibraries.svg)](https://kgysoft.net/corelibraries)
[![Online Help](https://img.shields.io/website/https/docs.kgysoft.net/corelibraries.svg?label=online%20help&up_message=available)](https://docs.kgysoft.net/corelibraries)
[![CoreLibraries Repo](https://img.shields.io/github/repo-size/koszeggy/KGySoft.CoreLibraries.svg?label=CoreLibraries)](https://github.com/koszeggy/KGySoft.CoreLibraries)

![KGySoft.ComponentModelDemo](https://kgysoft.net/images/KGySoft.ComponentModelDemo.jpg)

## A few highlights:
* The [KGySoft.ComponentModelDemo.Model](https://github.com/koszeggy/KGySoft.ComponentModelDemo/tree/master/KGySoft.ComponentModelDemo/Model) namespace demonstrates how to use the various [business object](https://kgysoft.net/corelibraries#business-objects) base types as model classes.
* It contains also some [example commands](https://github.com/koszeggy/KGySoft.ComponentModelDemo/blob/master/KGySoft.ComponentModelDemo/Model/Commands.cs). See more about KGy SOFT's technology agnostic commands and command bindings on the [website](https://kgysoft.net/corelibraries#command-binding).
* The [KGySoft.ComponentModelDemo.ViewModel](https://github.com/koszeggy/KGySoft.ComponentModelDemo/tree/master/KGySoft.ComponentModelDemo/ViewModel) namespace demonstrates how to create a technology-agnostic ViewModel. The [MainViewModel](https://github.com/koszeggy/KGySoft.ComponentModelDemo/tree/master/KGySoft.ComponentModelDemo/ViewModel/MainViewModel.cs) class is used by a [WPF Window](https://github.com/koszeggy/KGySoft.ComponentModelDemo/blob/master/KGySoft.ComponentModelDemo/ViewWpf/Windows/MainWindow.xaml) and [WinForms Form](https://github.com/koszeggy/KGySoft.ComponentModelDemo/blob/master/KGySoft.ComponentModelDemo/ViewWinForms/Forms/MainForm.cs) as well.

## Useful WPF Components
* The [KGyCommandAdapter](https://github.com/koszeggy/KGySoft.ComponentModelDemo/blob/master/KGySoft.ComponentModelDemo/ViewWpf/Commands/KGyCommandAdapter.cs) class makes possible to use KGy SOFT commands in WPF as traditional Microsoft commands.
* The [EventToCommand](https://github.com/koszeggy/KGySoft.ComponentModelDemo/blob/master/KGySoft.ComponentModelDemo/ViewWpf/Commands/EventToKGyCommandExtension.cs) markup extension makes possible to create bindings to KGy SOFT commands directly from XAML like this (see the [MainWindow.xaml](https://github.com/koszeggy/KGySoft.ComponentModelDemo/blob/master/KGySoft.ComponentModelDemo/ViewWpf/Windows/MainWindow.xaml) file for more examples):
```xaml
<Button Content="Click Me" Click="{commands:EventToKGyCommand Command={Binding DoSomethingCommand}}"/>
```
* The [EditToolBar](https://github.com/koszeggy/KGySoft.ComponentModelDemo/blob/master/KGySoft.ComponentModelDemo/ViewWpf/Controls/EditToolBar.xaml.cs) control can be bound to any undoable/editable object. It also demonstrates the usage of the [KGyCommandAdapter](https://github.com/koszeggy/KGySoft.ComponentModelDemo/blob/master/KGySoft.ComponentModelDemo/ViewWpf/Commands/KGyCommandAdapter.cs) class.
* The [ValidationResult](https://github.com/koszeggy/KGySoft.ComponentModelDemo/blob/master/KGySoft.ComponentModelDemo/ViewWpf/Validation/ValidationBindingExtension.cs) and [HasValidationResult](https://github.com/koszeggy/KGySoft.ComponentModelDemo/blob/master/KGySoft.ComponentModelDemo/ViewWpf/Validation/HasValidationResultExtension.cs) markup extensions make possible to obtain/check validation results of [IValidatingObject](http://docs.kgysoft.net/corelibraries/?topic=html/T_KGySoft_ComponentModel_IValidatingObject.htm) instances directly from XAML like this (see the [MainWindow.xaml](https://github.com/koszeggy/KGySoft.ComponentModelDemo/blob/master/KGySoft.ComponentModelDemo/ViewWpf/Windows/MainWindow.xaml) file for more examples):
```xaml
<DataTrigger Value="True" Binding="{validation:HasValidationResult Warning,
                                    Path=TestList/ValidationResults,
                                    PropertyName=IntProp}">
```
* The [ElementAdorner.Template](https://github.com/koszeggy/KGySoft.ComponentModelDemo/blob/master/KGySoft.ComponentModelDemo/ViewWpf/Adorners/ElementAdorner.cs) attached property can be used to define a template for a UIElement that will be displayed in the adorner layer. This makes possible creating templates for Warning and Information validation levels similarly to WPF's Validation.ErrorTemplate property (see the [MainWindow.xaml](https://github.com/koszeggy/KGySoft.ComponentModelDemo/blob/master/KGySoft.ComponentModelDemo/ViewWpf/Windows/MainWindow.xaml) file for more examples):
```xaml
<!-- Warning Template -->
<Setter Property="adorners:ElementAdorner.Template">
    <Setter.Value>
        <ControlTemplate>
            <Border BorderBrush="Orange" BorderThickness="3">
                <adorners:AdornedElementPlaceholder/>
            </Border>
        </ControlTemplate>
    </Setter.Value>
</Setter>
```

## Useful Windows Forms Components
* The [EditMenuStrip](https://github.com/koszeggy/KGySoft.ComponentModelDemo/blob/master/KGySoft.ComponentModelDemo/ViewWinForms/Controls/EditMenuStrip.cs) control can be bound to any undoable/editable object.
* The [ValidationResultToErrorProviderAdapter](https://github.com/koszeggy/KGySoft.ComponentModelDemo/blob/master/KGySoft.ComponentModelDemo/ViewWinForms/Components/ValidationResultToErrorProviderAdapter.cs) component can turn an ErrorProvider component to a WarningProvider or InfoProvider. Just drop it on the Windows Forms Designer, and select the provider instance and the severity. If the `DataSource` property provides [IValidatingObject](http://docs.kgysoft.net/corelibraries/?topic=html/T_KGySoft_ComponentModel_IValidatingObject.htm) instances, then the selected provider will display the validation results of the chosen severity.

## License
This repository is under the MIT license. You can freely reuse all code you find here even in commercial products without any restrictions.
