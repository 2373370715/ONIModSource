using System;

// Token: 0x020010DC RID: 4316
internal class TaskDivision<Task, SharedData> where Task : DivisibleTask<SharedData>, new()
{
	// Token: 0x060058A4 RID: 22692 RVA: 0x0028C910 File Offset: 0x0028AB10
	public TaskDivision(int taskCount)
	{
		this.tasks = new Task[taskCount];
		for (int num = 0; num != this.tasks.Length; num++)
		{
			this.tasks[num] = Activator.CreateInstance<Task>();
		}
	}

	// Token: 0x060058A5 RID: 22693 RVA: 0x000D9CDE File Offset: 0x000D7EDE
	public TaskDivision() : this(CPUBudget.coreCount)
	{
	}

	// Token: 0x060058A6 RID: 22694 RVA: 0x0028C954 File Offset: 0x0028AB54
	public void Initialize(int count)
	{
		int num = count / this.tasks.Length;
		for (int num2 = 0; num2 != this.tasks.Length; num2++)
		{
			this.tasks[num2].start = num2 * num;
			this.tasks[num2].end = this.tasks[num2].start + num;
		}
		DebugUtil.Assert(this.tasks[this.tasks.Length - 1].end + count % this.tasks.Length == count);
		this.tasks[this.tasks.Length - 1].end = count;
	}

	// Token: 0x060058A7 RID: 22695 RVA: 0x0028CA18 File Offset: 0x0028AC18
	public void Run(SharedData sharedData)
	{
		Task[] array = this.tasks;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Run(sharedData);
		}
	}

	// Token: 0x04003E84 RID: 16004
	public Task[] tasks;
}
