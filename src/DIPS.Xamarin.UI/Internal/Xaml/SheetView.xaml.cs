﻿using System;
using DIPS.Xamarin.UI.Controls.Sheet;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DIPS.Xamarin.UI.Internal.xaml
{
    /// <summary>
    /// A sheetview that is used inside of a <see cref="SheetBehavior"/>
    /// </summary>
    /// <remarks>This is a internal Xaml control that should only be used in a <see cref="SheetBehavior"/></remarks>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SheetView : ContentView
    {
        private readonly SheetBehavior m_sheetBehaviour;
        /// <summary>
        /// Constructs a <see cref="SheetView"/>
        /// </summary>
        /// <param name="sheetBehavior"></param>
        public SheetView(SheetBehavior sheetBehavior)
        {
            InitializeComponent();
            OuterSheetFrame.BindingContext = m_sheetBehaviour = sheetBehavior;
        }

        /// <summary>
        /// The height that the sheet content needs if it should display all of its content
        /// </summary>
        internal double SheetContentHeightRequest =>
            sheetContentView.Content != null
                ? SheetContentView.Content.Height + HandleBoxView.Height + OuterSheetFrame.Padding.Top + OuterSheetFrame.Padding.Bottom +
                  OuterSheetFrame.CornerRadius
                : 0;

        internal BoxView Handle => HandleBoxView;

        /// <summary>
        /// The internal outer sheet frame of the view
        /// </summary>
        internal Frame SheetFrame => OuterSheetFrame;

        internal ContentView SheetContentView => sheetContentView;

        private double m_newY;
        private void OnDrag(object sender, PanUpdatedEventArgs e)
        {
            if (!m_sheetBehaviour.IsDraggable) return;
            if (m_newY == 0) m_newY = SheetFrame.TranslationY;

            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    m_sheetBehaviour.IsDragging = true;
                    break;
                case GestureStatus.Running:

                    var translationY = (Device.RuntimePlatform == Device.Android) ? OuterSheetFrame.TranslationY : m_newY;
                    var newYTranslation = e.TotalY + translationY;
                    //Hack to remove jitter from android 
                    if (Device.RuntimePlatform == Device.Android)
                    {
                        e = new PanUpdatedEventArgs(e.StatusType, e.GestureId, 0, newYTranslation);
                        newYTranslation = e.TotalY;
                    }

                    m_sheetBehaviour.UpdatePosition(newYTranslation);
                    break;
                case GestureStatus.Completed:
                    m_newY = SheetFrame.TranslationY;
                    m_sheetBehaviour.IsDragging = false;
                    //Snap?
                    break;
                case GestureStatus.Canceled:
                    m_sheetBehaviour.IsDragging = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        internal void Initialize()
        {
            m_newY = 0;
            //Flp the grid if alignment is set to top
            if (m_sheetBehaviour.Alignment == AlignmentOptions.Top)
            {
                SheetGrid.RowDefinitions[0].Height = GridLength.Star;
                SheetGrid.RowDefinitions[1].Height = GridLength.Auto;
                Grid.SetRow(SheetContentGrid, 0);
                Grid.SetRow(HandleBoxView, 1);
            }
            else
            {
                SheetGrid.RowDefinitions[0].Height = GridLength.Auto;
                SheetGrid.RowDefinitions[1].Height = GridLength.Star;
                Grid.SetRow(HandleBoxView, 0);
                Grid.SetRow(SheetContentGrid, 1);
            }

            switch (m_sheetBehaviour.VerticalContentAlignment)
            {
                case ContentAlignment.Fit:
                    SheetContentGrid.VerticalOptions = m_sheetBehaviour.Alignment == AlignmentOptions.Top ? LayoutOptions.EndAndExpand : LayoutOptions.StartAndExpand;
                    break;
                case ContentAlignment.Fill:
                    SheetContentGrid.VerticalOptions = LayoutOptions.Fill;
                    break;
                default:
                    break;
            }
        }
    }
}