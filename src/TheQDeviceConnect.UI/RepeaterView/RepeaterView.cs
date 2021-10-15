using System;
using System.Collections;
using MvvmCross.Forms.Views;
using TheQDeviceConnect.Core.ViewModels;
using TheQDeviceConnect.Core.ViewModels.Connection;
using Xamarin.Forms;

namespace TheQDeviceConnect.UI.RepeaterView
{
    public class RepeaterView : StackLayout
    {
        public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(
            nameof(ItemTemplate),
            typeof(DataTemplateSelector),
            typeof(RepeaterView),
            default(DataTemplate));

        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
            nameof(ItemsSource),
            typeof(IEnumerable),
            typeof(RepeaterView),
            null,
            BindingMode.OneWay,
            propertyChanged: ItemsChanged);

        public RepeaterView()
        {
            Spacing = 0;
        }

        public ICollection ItemsSource
        {
            get => (ICollection)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public DataTemplateSelector ItemTemplate
        {
            get => (DataTemplateSelector)GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }

        protected virtual View ViewFor(object item)
        {
            View view = null;

            if (ItemTemplate != null)
            {
                var template = ItemTemplate.SelectTemplate(item, null);

                var content = template.CreateContent();


                view = content is View ? content as View : ((MvxViewCell)content).View;

                (content as MvxViewCell).ViewModel = (WifiNetworkViewModel)item;
                view.BindingContext = item;
            }

            return view;
        }

        private static void ItemsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is RepeaterView control))
                return;

            control.Children.Clear();

            var items = (ICollection)newValue;

            if (items == null)
                return;

            foreach (var item in items)
            {
                control.Children.Add(control.ViewFor(item));
            }
        }
    }
}
