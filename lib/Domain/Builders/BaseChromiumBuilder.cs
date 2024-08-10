﻿//  Copyright 2019-2024 Chris Mohan, Jaben Cargman
//  and GotenbergSharpApiClient Contributors
// 
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
// 
//      http://www.apache.org/licenses/LICENSE-2.0
// 
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.

namespace Gotenberg.Sharp.API.Client.Domain.Builders;

public abstract class BaseChromiumBuilder<TRequest, TBuilder>(TRequest request)
    : BaseBuilder<TRequest, TBuilder>(request)
    where TRequest : ChromeRequest
    where TBuilder : BaseChromiumBuilder<TRequest, TBuilder>
{
    public TBuilder WithDimensions(Action<DimensionBuilder> action)
    {
        if (action == null) throw new ArgumentNullException(nameof(action));

        var builder = new DimensionBuilder(this.Request.Dimensions);

        action(builder);

        this.Request.Dimensions = builder.GetDimensions();

        return (TBuilder)this;
    }

    public TBuilder WithDimensions(Dimensions dimensions)
    {
        this.Request.Dimensions = dimensions ?? throw new ArgumentNullException(nameof(dimensions));
        return (TBuilder)this;
    }

    public TBuilder WithAssets(Action<AssetBuilder> action)
    {
        if (action == null) throw new ArgumentNullException(nameof(action));
        action(new AssetBuilder(this.Request.Assets ??= new AssetDictionary()));
        return (TBuilder)this;
    }

    public TBuilder WithAsyncAssets(Func<AssetBuilder, Task> asyncAction)
    {
        if (asyncAction == null) throw new ArgumentNullException(nameof(asyncAction));
        this.BuildTasks.Add(
            asyncAction(new AssetBuilder(this.Request.Assets ??= new AssetDictionary())));
        return (TBuilder)this;
    }

    public TBuilder SetConversionBehaviors(Action<HtmlConversionBehaviorBuilder> action)
    {
        if (action == null) throw new ArgumentNullException(nameof(action));
        action(new HtmlConversionBehaviorBuilder(this.Request.ConversionBehaviors));
        return (TBuilder)this;
    }

    public TBuilder SetConversionBehaviors(HtmlConversionBehaviors behaviors)
    {
        this.Request.ConversionBehaviors =
            behaviors ?? throw new ArgumentNullException(nameof(behaviors));
        return (TBuilder)this;
    }
}