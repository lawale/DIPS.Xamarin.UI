using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace DIPS.Xamarin.UI.Controls.Pdf
{
    public class ZoomContainer : ContentView
    {
        private double m_startScale;
        private double m_currentScale;
        private double m_xOffset;
        private double m_yOffset;

        public ZoomContainer()
        {
            //var pinchGesture = new PinchGestureRecognizer();
            var panGesture = new PanGestureRecognizer();

            //pinchGesture.PinchUpdated += OnPinchUpdated;
            panGesture.PanUpdated += OnPanUpdated;

            //GestureRecognizers.Add(pinchGesture);
            GestureRecognizers.Add(panGesture);
        }

        private void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    break;
                case GestureStatus.Running:

                    var newYTranslation = e.TotalY + m_yOffset;
                    var newXTranslation = e.TotalX + m_xOffset;
                    //Hack to remove jitter from android 
                    if (Device.RuntimePlatform == Device.Android)
                    {
                        e = new PanUpdatedEventArgs(e.StatusType, e.GestureId, newXTranslation, newYTranslation);
                        newYTranslation = e.TotalY;
                        newXTranslation = e.TotalX;
                    }

                    Content.TranslationY = newYTranslation;
                    Content.TranslationX = newXTranslation;
                    break;
                case GestureStatus.Completed:
                    // Store the translation delta's of the wrapped user interface element.

                    //Content.TranslationY = e.TotalY;
                    //Content.TranslationX = e.TotalX;

                    m_xOffset = Content.TranslationX;
                    m_yOffset = Content.TranslationY;
                    //Snap?
                    break;
                case GestureStatus.Canceled:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        void OnPinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
        {
            if (e.Status == GestureStatus.Started)
            {
                // Store the current scale factor applied to the wrapped user interface element,
                // and zero the components for the center point of the translate transform.
                m_startScale = Content.Scale;
                Content.AnchorX = 0;
                Content.AnchorY = 0;
            }
            if (e.Status == GestureStatus.Running)
            {
                // Calculate the scale factor to be applied.
                m_currentScale += (e.Scale - 1) * m_startScale;
                m_currentScale = Math.Max(1, m_currentScale);

                // The ScaleOrigin is in relative coordinates to the wrapped user interface element,
                // so get the X pixel coordinate.
                double renderedX = Content.X + m_xOffset;
                double deltaX = renderedX / Width;
                double deltaWidth = Width / (Content.Width * m_startScale);
                double originX = (e.ScaleOrigin.X - deltaX) * deltaWidth;

                // The ScaleOrigin is in relative coordinates to the wrapped user interface element,
                // so get the Y pixel coordinate.
                double renderedY = Content.Y + m_yOffset;
                double deltaY = renderedY / Height;
                double deltaHeight = Height / (Content.Height * m_startScale);
                double originY = (e.ScaleOrigin.Y - deltaY) * deltaHeight;

                // Calculate the transformed element pixel coordinates.
                double targetX = m_xOffset - (originX * Content.Width) * (m_currentScale - m_startScale);
                double targetY = m_yOffset - (originY * Content.Height) * (m_currentScale - m_startScale);

                // Apply translation based on the change in origin.
                Content.TranslationX = targetX.Clamp(-Content.Width * (m_currentScale - 1), 0);
                Content.TranslationY = targetY.Clamp(-Content.Height * (m_currentScale - 1), 0);

                // Apply scale factor.
                Content.Scale = m_currentScale;
            }
            if (e.Status == GestureStatus.Completed)
            {
                // Store the translation delta's of the wrapped user interface element.
                m_xOffset = Content.TranslationX;
                m_yOffset = Content.TranslationY;
            }
        }

        public void Reset()
        {
            m_xOffset = 0;
            m_yOffset = 0;
        }
    }
}