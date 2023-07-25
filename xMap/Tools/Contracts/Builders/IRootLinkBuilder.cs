namespace xMap.Tools.Contracts;

internal interface IRootLinkBuilder
{
    ILinkBuilder StartWith<THandler>() where THandler : ILink;

    ILinkBuilder StartWith<THandler>(THandler handler) where THandler : ILink;
}