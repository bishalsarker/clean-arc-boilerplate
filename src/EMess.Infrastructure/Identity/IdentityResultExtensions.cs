﻿using Microsoft.AspNetCore.Identity;

namespace EMess.Infrastructure.Identity;

internal static class IdentityResultExtensions
{
    public static List<string> GetErrors(this IdentityResult result) =>
        result.Errors.Select(e => e.Description.ToString()).ToList();
}