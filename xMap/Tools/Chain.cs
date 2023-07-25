using Microsoft.CodeAnalysis;
using xMap.Tools.Contracts;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace xMap.Tools;

internal sealed class Chain
{
    private readonly ILink _root;

    private Chain(ILink rootLink)
    {
        _root = rootLink;
    }

    public static IRootLinkBuilder Builder => new RootLinkBuilder();

    public void Process(SourceProductionContext context, SemanticModel semanticModel)
    {
        _root?.Process(context, semanticModel, CompilationUnit());
    }

    private class RootLinkBuilder : IRootLinkBuilder, ILinkBuilder, ILastLinkBuilder
    {
        private ILink _currentLink = null!;
        private ILink _nextLink = null!;
        private ILink _root = null!;

        public Chain BuildChain()
        {
            return new Chain(_root);
        }

        public ILinkBuilder Then<THandler>() where THandler : ILink
        {
            _nextLink = (Activator.CreateInstance(typeof(THandler)) as ILink)!;
            MoveLinks(ref _currentLink, _nextLink);

            return this;
        }

        public ILinkBuilder Then<THandler>(THandler handler) where THandler : ILink
        {
            _nextLink = handler;
            MoveLinks(ref _currentLink, _nextLink);

            return this;
        }

        public ILastLinkBuilder FinishWith<THandler>() where THandler : ILink
        {
            _nextLink = (Activator.CreateInstance(typeof(THandler)) as ILink)!;
            MoveLinks(ref _currentLink, _nextLink);

            return this;
        }

        public ILastLinkBuilder FinishWith<THandler>(THandler handler) where THandler : ILink
        {
            _nextLink = handler;
            MoveLinks(ref _currentLink, _nextLink);

            return this;
        }

        public ILinkBuilder StartWith<THandler>() where THandler : ILink
        {
            _root = (Activator.CreateInstance(typeof(THandler)) as ILink)!;
            _currentLink = _root;
            _nextLink = _currentLink.Next;

            return this;
        }

        public ILinkBuilder StartWith<THandler>(THandler handler) where THandler : ILink
        {
            _root = handler;
            _currentLink = _root;
            _nextLink = _currentLink.Next;

            return this;
        }

        private static void MoveLinks(ref ILink current, ILink next)
        {
            current.Next = next;
            current = current.Next;
        }
    }
}