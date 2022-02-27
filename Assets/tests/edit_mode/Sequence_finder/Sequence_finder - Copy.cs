// using System.Collections;
// using System.Collections.Generic;
// using Moq;
// using NUnit.Framework;
// using NUnit.Framework.Internal;
// using rvinowise.ai.general;
// using rvinowise.ai.unity;
// using rvinowise.ai.unity.mapping_stencils;
// using UnityEngine;
// using UnityEngine.TestTools;
//
// namespace rvinowise.ai.unit_tests.sequence_finder2 {
//
// [TestFixture]
// public partial class sequences_can_be_found {
//     
//     private Mock<Action_history> action_history_mock;
//     private IReadOnlyList<IAction_group> raw_action_groups;
//     private Figure_storage storage;
//     private Network_initialiser network_initialiser;
//     [SetUp]
//     public void prepare_raw_signals() {
//         network_initialiser = new GameObject().AddComponent<Network_initialiser>();
//         network_initialiser.figure_storage = storage;
//         network_initialiser.create_base_signals();
//         
//         Action_history history = new GameObject().AddComponent<Action_history>();
//         for(int i=0;i<max_groups;i++) {
//             history.create_next_action_group(0);
//         }
//         history.create_figure_appearance()
//     }
//
//     private void prepare_figure_storage(Figure_storage storage) {
//         storage = new GameObject().AddComponent<Figure_storage>();
//         storage.append_figure();
//     }
//
//     const int max_groups = 10;
//     private IReadOnlyList<IAction_group> prepare_action_groups() {
//         List<IAction_group> groups = new List<IAction_group>();
//         for (int i_group = 0;i_group < max_groups; i_group++) {
//             var group = new GameObject().AddComponent<Action_group>();
//             groups.Add(group);
//         }
//         Mock<IAction> action_mock;
//         action_mock = new Mock<IAppearance_start>();
//         action_mock.Setup(a =>
//             a.figure.id).Returns("0");
//         groups[0].add_action(action_mock.Object);
//         action_mock = new Mock<IAppearance_end>();
//         action_mock.Setup(a =>
//             a.figure.id).Returns("0");
//         groups[1].add_action(action_mock.Object);
//         
//         return groups.AsReadOnly();
//     }
//     
//     [Test]
//     public void find_repeated_pairs() {
//         
//         
//         Sequence_finder sequence_finder = new GameObject().AddComponent<Sequence_finder>();
//         sequence_finder.action_history = action_history_mock.Object;
//         
//         sequence_finder.enrich_storage_with_sequences();
//         
//         Debug.Log("random gave: ");
//
//         // Sequence_finder sequence_finder = new GameObject().AddComponent<Sequence_finder>();
//         //
//         // var action_history = new Mock<IAction_history>();
//         //
//         // sequence_finder.action_history = action_history;
//         // sequence_finder.figure_storage = figure_storage;
//         // sequence_finder.sequence_builder = sequence_builder;
//         //
//         // sequence_finder.enrich_storage_with_sequences();
//         //
//         // int i_combination = 0;
//         // while (combinator.MoveNext()) {
//         //     Debug.Log(
//         //         string.Join(", ", combinator.combination)
//         //     );
//         //     CollectionAssert.AreEquivalent(
//         //         result_combinations[i_combination++],
//         //         combinator.combination
//         //     );
//         // }
//         //
//         // Assert.AreEqual(result_combinations.Length, i_combination);
//     }
//     
//     [Test]
//     public void find_repeated_pairs_0() {
//         // Sequence_finder sequence_finder = new GameObject().AddComponent<Sequence_finder>();
//         // Action_history action_history = new GameObject().AddComponent<Action_history>();
//         // Figure_storage figure_storage = new GameObject().AddComponent<Figure_storage>();
//         // Sequence_builder sequence_builder = new GameObject().AddComponent<Sequence_builder>();
//         //
//         // sequence_finder.action_history = action_history;
//         // sequence_finder.figure_storage = figure_storage;
//         // sequence_finder.sequence_builder = sequence_builder;
//         //
//         // sequence_finder.enrich_storage_with_sequences();
//         //
//         // int i_combination = 0;
//         // while (combinator.MoveNext()) {
//         //     Debug.Log(
//         //         string.Join(", ", combinator.combination)
//         //     );
//         //     CollectionAssert.AreEquivalent(
//         //         result_combinations[i_combination++],
//         //         combinator.combination
//         //     );
//         // }
//         //
//         // Assert.AreEqual(result_combinations.Length, i_combination);
//     }
// }
//
//
// }