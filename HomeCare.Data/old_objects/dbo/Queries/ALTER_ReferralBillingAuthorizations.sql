ALTER TABLE ReferralBillingAuthorizations
ADD 
	ServiceCodeID BIGINT NULL,
	Rate decimal NULL,
	RevenueCode BIGINT NULL,
	UnitType INT NULL,
	PerUnitQuantity DECIMAL NULL,
	RoundUpUnit INT NULL,
	MaxUnit INT NULL,
	DailyUnitLimit INT NULL,
	CareType BIGINT NULL;