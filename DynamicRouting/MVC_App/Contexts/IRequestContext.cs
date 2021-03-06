﻿using CMS.DocumentEngine;

namespace Custom.DynamicRouting.Contexts
{
    public interface IRequestContext
    {
        int? ContextItemId { get; set; }

        bool ContextResolved { get; set; }

        bool IsPreview { get; set; }

        T GetContextItem<T>() where T : TreeNode, new();
    }
}
