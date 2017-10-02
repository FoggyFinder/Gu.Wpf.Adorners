﻿namespace Gu.Wpf.Adorners
{
    using System;
    using System.Windows;

    /// <summary>
    /// Manager for the DependencyObject.Loaded event.
    /// </summary>
    internal class LoadedEventManager : WeakEventManager
    {
        private LoadedEventManager()
        {
        }

        // get the event manager for the current thread
        private static LoadedEventManager CurrentManager
        {
            get
            {
                var managerType = typeof(LoadedEventManager);
                var manager = (LoadedEventManager)GetCurrentManager(managerType);

                // at first use, create and register a new manager
                if (manager == null)
                {
                    manager = new LoadedEventManager();
                    SetCurrentManager(managerType, manager);
                }

                return manager;
            }
        }

        /// <summary>
        /// Add a listener to the given source's event.
        /// </summary>
        public static void AddListener(DependencyObject source, IWeakEventListener listener)
        {
            CurrentManager.ProtectedAddListener(
                source ?? throw new ArgumentNullException(nameof(source)),
                listener ?? throw new ArgumentNullException(nameof(listener)));
        }

        /// <summary>
        /// Remove a listener to the given source's event.
        /// </summary>
        public static void RemoveListener(DependencyObject source, IWeakEventListener listener)
        {
            CurrentManager.ProtectedRemoveListener(
                source ?? throw new ArgumentNullException(nameof(source)),
                listener ?? throw new ArgumentNullException(nameof(listener)));
        }

        /// <summary>
        /// Add a handler for the given source's event.
        /// </summary>
        public static void AddHandler(DependencyObject source, EventHandler<RoutedEventArgs> handler)
        {
            CurrentManager.ProtectedAddHandler(
                source,
                handler ?? throw new ArgumentNullException(nameof(handler)));
        }

        /// <summary>
        /// Remove a handler for the given source's event.
        /// </summary>
        public static void RemoveHandler(DependencyObject source, EventHandler<RoutedEventArgs> handler)
        {
            CurrentManager.ProtectedRemoveHandler(
                source,
                handler ?? throw new ArgumentNullException(nameof(handler)));
        }

        /// <inheritdoc />
        protected override ListenerList NewListenerList() => new ListenerList<RoutedEventArgs>();

        /// <inheritdoc />
        protected override void StartListening(object source)
        {
            if (source is FrameworkElement fe)
            {
                fe.Loaded += this.OnLoaded;
            }
            else if (source is FrameworkContentElement fce)
            {
                fce.Loaded += this.OnLoaded;
            }
            else
            {
                throw new ArgumentException($"Cannot start listening to {source?.GetType().Name ?? "null"}");
            }
        }

        /// <inheritdoc />
        protected override void StopListening(object source)
        {
            if (source is FrameworkElement fe)
            {
                fe.Loaded -= this.OnLoaded;
            }
            else if (source is FrameworkContentElement fce)
            {
                fce.Loaded -= this.OnLoaded;
            }
            else
            {
                throw new ArgumentException($"Cannot stop listening to {source?.GetType().Name ?? "null"}");
            }
        }

        // event handler for Loaded event
        private void OnLoaded(object sender, RoutedEventArgs args)
        {
            this.DeliverEvent(sender, args);
        }
    }
}