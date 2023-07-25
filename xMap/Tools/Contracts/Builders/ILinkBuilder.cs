namespace xMap.Tools.Contracts;

internal interface ILinkBuilder
{
    ILinkBuilder Then<THandler>() where THandler : ILink;

    ILinkBuilder Then<THandler>(THandler handler) where THandler : ILink;

    ILastLinkBuilder FinishWith<THandler>() where THandler : ILink;

    ILastLinkBuilder FinishWith<THandler>(THandler handler) where THandler : ILink;
}