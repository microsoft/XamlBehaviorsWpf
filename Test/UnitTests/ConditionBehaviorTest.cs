// -------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All Rights Reserved.
// -------------------------------------------------------------------
namespace Microsoft.Xaml.Interactions.UnitTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Xaml.Interactions.Core;
    using Microsoft.Xaml.Interactivity;

    [TestClass]
    public class ConditionBehaviorTest
    {
        [TestInitialize]
        public void Setup()
        {
            Interaction.ShouldRunInDesignMode = true;
        }

        [TestCleanup]
        public void Teardown()
        {
            Interaction.ShouldRunInDesignMode = false;
        }

        private static void SetupTriggerActionConditionBehavior(out ConditionalExpression conditionalExpression, out StubTrigger trigger, out StubAction action)
        {
            ConditionBehavior conditionBehavior = new ConditionBehavior();
            conditionalExpression = new ConditionalExpression();
            // Hook the conditional expresison to the condition behavior
            conditionBehavior.Condition = conditionalExpression;

            trigger = new StubTrigger();
            BehaviorCollection behaviorCollection = Interaction.GetBehaviors(trigger);
            // Add the condition behavior to the trigger
            behaviorCollection.Add(conditionBehavior);

            // Addting an action to the trigger
            action = new StubAction();
            trigger.Actions.Add(action);
        }

        [TestMethod]
        public void InvokeTriggerWithConditionalBehavior_OneNotMetCondition()
        {
            ConditionalExpression conditionalExpression = null;
            StubTrigger trigger = null;
            StubAction action = null;
            SetupTriggerActionConditionBehavior(out conditionalExpression, out trigger, out action);

            conditionalExpression.Conditions.Add(new ComparisonCondition());
            conditionalExpression.Conditions[0].LeftOperand = BehaviorTestUtilities.IntegerOperand4;
            conditionalExpression.Conditions[0].RightOperand = BehaviorTestUtilities.IntegerOperand5;
            conditionalExpression.Conditions[0].Operator = ComparisonConditionType.GreaterThan;

            // Firing trigger with condition behavior not met
            trigger.FireStubTrigger();
            Assert.AreEqual(action.InvokeCount, 0, "action.InvokeCount == 0, conditon not met");
        }

        [TestMethod]
        public void InvokeTriggerWithConditionalBehavior_OneMetCondition()
        {
            ConditionalExpression conditionalExpression = null;
            StubTrigger trigger = null;
            StubAction action = null;
            SetupTriggerActionConditionBehavior(out conditionalExpression, out trigger, out action);

            conditionalExpression.Conditions.Add(new ComparisonCondition());
            conditionalExpression.Conditions[0].LeftOperand = BehaviorTestUtilities.IntegerOperand4;
            conditionalExpression.Conditions[0].RightOperand = BehaviorTestUtilities.IntegerOperand5;
            conditionalExpression.Conditions[0].Operator = ComparisonConditionType.LessThan;

            // Firing trigger with condition behavior met
            trigger.FireStubTrigger();
            Assert.AreEqual(action.InvokeCount, 1, "action.InvokeCount == 1, conditon met");
        }

        [TestMethod]
        public void InvokeTriggerWithConditionalBehavior_TwoConditionsOneNotMet()
        {
            ConditionalExpression conditionalExpression = null;
            StubTrigger trigger = null;
            StubAction action = null;
            SetupTriggerActionConditionBehavior(out conditionalExpression, out trigger, out action);

            // Resetting an non met condition
            conditionalExpression.Conditions.Add(new ComparisonCondition());
            conditionalExpression.Conditions[0].LeftOperand = BehaviorTestUtilities.IntegerOperand4;
            conditionalExpression.Conditions[0].RightOperand = BehaviorTestUtilities.IntegerOperand5;
            conditionalExpression.Conditions[0].Operator = ComparisonConditionType.GreaterThan;

            // Add the second condition
            conditionalExpression.Conditions.Add(new ComparisonCondition());
            Assert.IsTrue(conditionalExpression.Conditions.Count == 2, "We should have 2 conditions");

            // Creating a new condition
            conditionalExpression.Conditions[1].LeftOperand = BehaviorTestUtilities.IntegerOperand5;
            conditionalExpression.Conditions[1].RightOperand = BehaviorTestUtilities.IntegerOperand6;
            conditionalExpression.Conditions[1].Operator = ComparisonConditionType.LessThan;


            // Firing the trigger, with one of the condition not met
            trigger.FireStubTrigger();
            Assert.AreEqual(action.InvokeCount, 0, "action.InvokeCount == 0, one conditon not met");
        }

        [TestMethod]
        public void InvokeTriggerWithConditionalBehavior_TwoConditionsOneNotMetForwardChainingOr()
        {
            ConditionalExpression conditionalExpression = null;
            StubTrigger trigger = null;
            StubAction action = null;
            SetupTriggerActionConditionBehavior(out conditionalExpression, out trigger, out action);

            // Resetting an non met condition
            conditionalExpression.Conditions.Add(new ComparisonCondition());
            conditionalExpression.Conditions[0].LeftOperand = BehaviorTestUtilities.IntegerOperand4;
            conditionalExpression.Conditions[0].RightOperand = BehaviorTestUtilities.IntegerOperand5;
            conditionalExpression.Conditions[0].Operator = ComparisonConditionType.GreaterThan;

            // Add the second condition
            conditionalExpression.Conditions.Add(new ComparisonCondition());
            Assert.IsTrue(conditionalExpression.Conditions.Count == 2, "We should have 2 conditions");

            // Creating a new condition
            conditionalExpression.Conditions[1].LeftOperand = BehaviorTestUtilities.IntegerOperand5;
            conditionalExpression.Conditions[1].RightOperand = BehaviorTestUtilities.IntegerOperand6;
            conditionalExpression.Conditions[1].Operator = ComparisonConditionType.LessThan;

            // Firing the trigger, forward chaining changed to OR. 
            conditionalExpression.ForwardChaining = ForwardChaining.Or;
            trigger.FireStubTrigger();
            Assert.AreEqual(action.InvokeCount, 1, "action.InvokeCount == 1, one conditon is met, forward chaining was Or");
        }

        [TestMethod]
        public void InvokeTriggerWithConditionalBehavior_TwoConditionsBothNotMetForwardChainingOr()
        {
            ConditionalExpression conditionalExpression = null;
            StubTrigger trigger = null;
            StubAction action = null;
            SetupTriggerActionConditionBehavior(out conditionalExpression, out trigger, out action);

            // Resetting an non met condition
            conditionalExpression.Conditions.Add(new ComparisonCondition());
            conditionalExpression.Conditions[0].LeftOperand = BehaviorTestUtilities.IntegerOperand4;
            conditionalExpression.Conditions[0].RightOperand = BehaviorTestUtilities.IntegerOperand5;
            conditionalExpression.Conditions[0].Operator = ComparisonConditionType.GreaterThan;

            // Add the second condition
            conditionalExpression.Conditions.Add(new ComparisonCondition());
            Assert.IsTrue(conditionalExpression.Conditions.Count == 2, "We should have 2 conditions");

            // Creating a new condition
            conditionalExpression.Conditions[1].LeftOperand = BehaviorTestUtilities.IntegerOperand5;
            conditionalExpression.Conditions[1].RightOperand = BehaviorTestUtilities.IntegerOperand6;
            conditionalExpression.Conditions[1].Operator = ComparisonConditionType.GreaterThan;

            // Firing the trigger, forward chaining changed to OR. 
            conditionalExpression.ForwardChaining = ForwardChaining.Or;
            trigger.FireStubTrigger();
            Assert.AreEqual(action.InvokeCount, 0, "action.InvokeCount == 0, both conditons are not met, forward chaining was Or");
        }

        [TestMethod]
        public void InvokeTriggerWithConditionalBehavior_TwoConditionsMet()
        {
            ConditionalExpression conditionalExpression = null;
            StubTrigger trigger = null;
            StubAction action = null;
            SetupTriggerActionConditionBehavior(out conditionalExpression, out trigger, out action);

            conditionalExpression.Conditions.Add(new ComparisonCondition());
            conditionalExpression.Conditions[0].LeftOperand = BehaviorTestUtilities.IntegerOperand5;
            conditionalExpression.Conditions[0].RightOperand = BehaviorTestUtilities.IntegerOperand5;
            conditionalExpression.Conditions[0].Operator = ComparisonConditionType.Equal;
            conditionalExpression.Conditions.Add(new ComparisonCondition());
            conditionalExpression.Conditions[1].LeftOperand = BehaviorTestUtilities.StringOperandLoremIpsum;
            conditionalExpression.Conditions[1].RightOperand = BehaviorTestUtilities.StringOperandNuncViverra;
            conditionalExpression.Conditions[1].Operator = ComparisonConditionType.NotEqual;

            // Firing trigger with 2 conditions, two conditions are met
            trigger.FireStubTrigger();
            Assert.AreEqual(action.InvokeCount, 1, "action.InvokeCount == 1, both conditons met");
        }
    }
}
