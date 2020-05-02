﻿using System.IO;
using System.Numerics;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SatisfactorySaveParser.Save;
using SatisfactorySaveParser.Save.Serialization;

namespace SatisfactorySaveParser.Tests.Save
{
    [TestClass]
    public class ObjectHeaderTests
    {
        private static readonly byte[] ActorHeaderBytes = new byte[] { 0x01, 0x00, 0x00, 0x00, 0x5C, 0x00, 0x00, 0x00, 0x2F, 0x47, 0x61, 0x6D, 0x65, 0x2F, 0x46, 0x61, 0x63, 0x74, 0x6F, 0x72, 0x79, 0x47, 0x61, 0x6D, 0x65, 0x2F, 0x45, 0x71, 0x75, 0x69, 0x70, 0x6D, 0x65, 0x6E, 0x74, 0x2F, 0x43, 0x34, 0x44, 0x69, 0x73, 0x70, 0x65, 0x6E, 0x73, 0x65, 0x72, 0x2F, 0x42, 0x50, 0x5F, 0x44, 0x65, 0x73, 0x74, 0x72, 0x75, 0x63, 0x74, 0x69, 0x62, 0x6C, 0x65, 0x4C, 0x61, 0x72, 0x67, 0x65, 0x52, 0x6F, 0x63, 0x6B, 0x2E, 0x42, 0x50, 0x5F, 0x44, 0x65, 0x73, 0x74, 0x72, 0x75, 0x63, 0x74, 0x69, 0x62, 0x6C, 0x65, 0x4C, 0x61, 0x72, 0x67, 0x65, 0x52, 0x6F, 0x63, 0x6B, 0x5F, 0x43, 0x00, 0x11, 0x00, 0x00, 0x00, 0x50, 0x65, 0x72, 0x73, 0x69, 0x73, 0x74, 0x65, 0x6E, 0x74, 0x5F, 0x4C, 0x65, 0x76, 0x65, 0x6C, 0x00, 0x3E, 0x00, 0x00, 0x00, 0x50, 0x65, 0x72, 0x73, 0x69, 0x73, 0x74, 0x65, 0x6E, 0x74, 0x5F, 0x4C, 0x65, 0x76, 0x65, 0x6C, 0x3A, 0x50, 0x65, 0x72, 0x73, 0x69, 0x73, 0x74, 0x65, 0x6E, 0x74, 0x4C, 0x65, 0x76, 0x65, 0x6C, 0x2E, 0x42, 0x50, 0x5F, 0x44, 0x65, 0x73, 0x74, 0x72, 0x75, 0x63, 0x74, 0x69, 0x62, 0x6C, 0x65, 0x4C, 0x61, 0x72, 0x67, 0x65, 0x52, 0x6F, 0x63, 0x6B, 0x35, 0x38, 0x5F, 0x32, 0x00, 0x01, 0x00, 0x00, 0x00, 0x3F, 0xD3, 0xEF, 0x3E, 0x5D, 0xB3, 0x42, 0x3F, 0x0B, 0x71, 0x71, 0x3E, 0x3E, 0x03, 0xC4, 0xBE, 0x80, 0x80, 0xB3, 0xC7, 0x80, 0x53, 0x89, 0x48, 0xE4, 0xFC, 0x89, 0xC5, 0x00, 0x00, 0x80, 0x3F, 0x00, 0x00, 0x80, 0x3F, 0x00, 0x00, 0x80, 0x3F, 0x01, 0x00, 0x00, 0x00 };
        private const SaveObjectKind ActorHeaderKind = SaveObjectKind.Actor;
        private const string ActorHeaderTypePath = "/Game/FactoryGame/Equipment/C4Dispenser/BP_DestructibleLargeRock.BP_DestructibleLargeRock_C";
        private const string ActorHeaderLevelName = "Persistent_Level";
        private const string ActorHeaderPathName = "Persistent_Level:PersistentLevel.BP_DestructibleLargeRock58_2";
        private const bool ActorNeedTransform = true;
        private static readonly Vector4 ActorRotation = new Vector4(0.468408554792404f, 0.760549366474152f, 0.235782787203789f, -0.382837235927582f);
        private static readonly Vector3 ActorPosition = new Vector3(-91905f, 281244f, -4415.611328125f);
        private static readonly Vector3 ActorScale = new Vector3(1f, 1f, 1f);
        private const bool ActorWasPlacedInLevel = true;

        private static readonly byte[] ComponentHeaderBytes = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x2E, 0x00, 0x00, 0x00, 0x2F, 0x53, 0x63, 0x72, 0x69, 0x70, 0x74, 0x2F, 0x46, 0x61, 0x63, 0x74, 0x6F, 0x72, 0x79, 0x47, 0x61, 0x6D, 0x65, 0x2E, 0x46, 0x47, 0x49, 0x6E, 0x76, 0x65, 0x6E, 0x74, 0x6F, 0x72, 0x79, 0x43, 0x6F, 0x6D, 0x70, 0x6F, 0x6E, 0x65, 0x6E, 0x74, 0x54, 0x72, 0x61, 0x73, 0x68, 0x00, 0x11, 0x00, 0x00, 0x00, 0x50, 0x65, 0x72, 0x73, 0x69, 0x73, 0x74, 0x65, 0x6E, 0x74, 0x5F, 0x4C, 0x65, 0x76, 0x65, 0x6C, 0x00, 0x3B, 0x00, 0x00, 0x00, 0x50, 0x65, 0x72, 0x73, 0x69, 0x73, 0x74, 0x65, 0x6E, 0x74, 0x5F, 0x4C, 0x65, 0x76, 0x65, 0x6C, 0x3A, 0x50, 0x65, 0x72, 0x73, 0x69, 0x73, 0x74, 0x65, 0x6E, 0x74, 0x4C, 0x65, 0x76, 0x65, 0x6C, 0x2E, 0x43, 0x68, 0x61, 0x72, 0x5F, 0x50, 0x6C, 0x61, 0x79, 0x65, 0x72, 0x5F, 0x43, 0x5F, 0x30, 0x2E, 0x54, 0x72, 0x61, 0x73, 0x68, 0x53, 0x6C, 0x6F, 0x74, 0x00, 0x31, 0x00, 0x00, 0x00, 0x50, 0x65, 0x72, 0x73, 0x69, 0x73, 0x74, 0x65, 0x6E, 0x74, 0x5F, 0x4C, 0x65, 0x76, 0x65, 0x6C, 0x3A, 0x50, 0x65, 0x72, 0x73, 0x69, 0x73, 0x74, 0x65, 0x6E, 0x74, 0x4C, 0x65, 0x76, 0x65, 0x6C, 0x2E, 0x43, 0x68, 0x61, 0x72, 0x5F, 0x50, 0x6C, 0x61, 0x79, 0x65, 0x72, 0x5F, 0x43, 0x5F, 0x30, 0x00 };
        private const SaveObjectKind ComponentHeaderKind = SaveObjectKind.Component;
        private const string ComponentHeaderTypePath = "/Script/FactoryGame.FGInventoryComponentTrash";
        private const string ComponentHeaderLevelName = "Persistent_Level";
        private const string ComponentHeaderPathName = "Persistent_Level:PersistentLevel.Char_Player_C_0.TrashSlot";
        private const string ComponentHeaderParentEntityName = "Persistent_Level:PersistentLevel.Char_Player_C_0";

        [TestMethod]
        public void ObjectHeaderReading()
        {
            using var stream = new MemoryStream(ActorHeaderBytes);
            using var reader = new BinaryReader(stream);

            var header = SatisfactorySaveSerializer.DeserializeObjectHeader(reader);

            Assert.AreEqual(ActorHeaderTypePath, header.TypePath);
            Assert.AreEqual(ActorHeaderLevelName, header.Instance.LevelName);
            Assert.AreEqual(ActorHeaderPathName, header.Instance.PathName);

            Assert.AreEqual(stream.Length, stream.Position);
        }

        [TestMethod]
        public void ActorHeaderReading()
        {
            using var stream = new MemoryStream(ActorHeaderBytes);
            using var reader = new BinaryReader(stream);

            var header = (SaveActor)SatisfactorySaveSerializer.DeserializeObjectHeader(reader);

            Assert.AreEqual(ActorHeaderKind, header.ObjectKind);
            Assert.AreEqual(ActorNeedTransform, header.NeedTransform);
            Assert.AreEqual(ActorRotation, header.Rotation);
            Assert.AreEqual(ActorPosition, header.Position);
            Assert.AreEqual(ActorScale, header.Scale);
            Assert.AreEqual(ActorWasPlacedInLevel, header.WasPlacedInLevel);

            Assert.AreEqual(stream.Length, stream.Position);
        }

        [TestMethod]
        public void ActorHeaderWriting()
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            var header = new SaveActor
            {
                TypePath = ActorHeaderTypePath,
                Instance = new ObjectReference(ActorHeaderLevelName, ActorHeaderPathName),

                NeedTransform = ActorNeedTransform,
                Rotation = ActorRotation,
                Position = ActorPosition,
                Scale = ActorScale,
                WasPlacedInLevel = ActorWasPlacedInLevel
            };

            SatisfactorySaveSerializer.SerializeObjectHeader(header, writer);

            CollectionAssert.AreEqual(ActorHeaderBytes, stream.ToArray());
        }

        [TestMethod]
        public void ComponentHeaderReading()
        {
            using var stream = new MemoryStream(ComponentHeaderBytes);
            using var reader = new BinaryReader(stream);

            var header = (SaveComponent)SatisfactorySaveSerializer.DeserializeObjectHeader(reader);

            Assert.AreEqual(ComponentHeaderKind, header.ObjectKind);
            Assert.AreEqual(ComponentHeaderParentEntityName, header.ParentEntityName);

            Assert.AreEqual(stream.Length, stream.Position);
        }

        [TestMethod]
        public void ComponentHeaderWriting()
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            var header = new SaveComponent
            {
                TypePath = ComponentHeaderTypePath,
                Instance = new ObjectReference(ComponentHeaderLevelName, ComponentHeaderPathName),

                ParentEntityName = ComponentHeaderParentEntityName
            };

            SatisfactorySaveSerializer.SerializeObjectHeader(header, writer);

            CollectionAssert.AreEqual(ComponentHeaderBytes, stream.ToArray());
        }
    }
}
