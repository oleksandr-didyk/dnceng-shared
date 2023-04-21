// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.Options;
using Octokit;
using System;

namespace Microsoft.DotNet.GitHub.Authentication;

public class GitHubClientFactory : IGitHubClientFactory
{
    private readonly IOptionsMonitor<GitHubClientOptions> _githubClientOptions;

    public GitHubClientFactory(IOptionsMonitor<GitHubClientOptions> githubClientOptions)
    {
        _githubClientOptions = githubClientOptions;
    }

    public GitHubClientOptions Options => _githubClientOptions.CurrentValue; 

    public IGitHubClient CreateGitHubClient(string token)
    {
        return CreateGitHubClient(token, AuthenticationType.Oauth);
    }

    public IGitHubClient CreateGitHubClient(string token, AuthenticationType type)
    {
        if (Options?.ProductHeader == null)
        {
            throw new InvalidOperationException($"A {nameof(GitHubClientOptions.ProductHeader)} is required for a GitHub client, but the value in {nameof(GitHubClientOptions)} is null.");
        }

        var client = new GitHubClient(Options.ProductHeader);

        if (!string.IsNullOrEmpty(token))
        {
            client.Credentials = new Credentials(token, type);
        }

        return client;
    }
}
