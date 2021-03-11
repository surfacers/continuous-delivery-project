using System;
using GongSolutions.Wpf.DragDrop;

namespace Hurace.RaceControl.Extensions
{
    public static class IDropInfoExtensions
    {
        internal class DragDropAction<T>
        {
            public T Item { get; set; }

            public DragDropSource Drag { get; set; }

            public DragDropSource Drop { get; set; }
        }

        internal enum DragDropSource
        {
            Source, Target
        }

        internal static DragDropAction<T> GetDragDropAction<T>(this IDropInfo dropInfo, Func<T, bool> inSource)
            where T : class
        {
            var sourceItem = dropInfo.Data as T;
            var targetItem = dropInfo.TargetItem as T;

            if (sourceItem == null || targetItem == null) return null;

            return new DragDropAction<T>
            {
                Item = sourceItem,
                Drag = inSource(sourceItem) ? DragDropSource.Source : DragDropSource.Target,
                Drop = inSource(targetItem) ? DragDropSource.Source : DragDropSource.Target
            };
        }

        internal static int GetIndex(this IDropInfo dropInfo)
        {
            return dropInfo.InsertPosition == RelativeInsertPosition.AfterTargetItem
                ? dropInfo.InsertIndex - 1
                : dropInfo.InsertIndex;
        }
    }
}
