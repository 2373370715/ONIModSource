using System;

// Token: 0x02001F59 RID: 8025
public interface IConfigurableConsumer
{
	// Token: 0x0600A967 RID: 43367
	IConfigurableConsumerOption[] GetSettingOptions();

	// Token: 0x0600A968 RID: 43368
	IConfigurableConsumerOption GetSelectedOption();

	// Token: 0x0600A969 RID: 43369
	void SetSelectedOption(IConfigurableConsumerOption option);
}
