

pub fn ConvertToDouble(val: Option<Box<dyn std::any::Any>>) -> Option<f64>
{
	if let Some(dval) = val
	{
		match dval.downcast::<f64>()
		{
			Ok(x) => {
				return Some(*x);
			},
			Err(dval) => {
				match dval.downcast::<i64>()
				{
					Ok(x) => {
						return Some(*x as f64);
					},
					Err(notdouble) => {
						match notdouble.downcast::<bool>()
						{
							Ok(x) => {
								return Some(if *x { 1.0 } else { 0.0 });
							},
							Err(notbool) => {
								match notbool.downcast::<String>()
								{
									Ok(x) => {
										match (*x).parse::<f64>()
										{
											Ok(v) => return Some(v),
											Err(_e) => {}
										}
									},
									Err(_e) => {
										
									}
								}
							}
						}
					}
				}
			}
		}
	}
	None
}
pub fn ConvertToBoolean(val: Option<Box<dyn std::any::Any>>) -> Option<bool>
{
	if let Some(dval) = val
	{
		match dval.downcast::<bool>()
		{
			Ok(x) => {
				return Some(*x);
			},
			Err(dval) => {
				match dval.downcast::<f64>()
				{
					Ok(x) => {
						return Some(*x != 0.0);
					},
					Err(notdouble) => {
						match notdouble.downcast::<i64>()
						{
							Ok(x) => {
								return Some(*x != 0);
							},
							Err(notbool) => {
								match notbool.downcast::<String>()
								{
									Ok(x) => {
										match (*x).parse::<bool>()
										{
											Ok(v) => return Some(v),
											Err(_e) => {}
										}
									},
									Err(_e) => {
										
									}
								}
							}
						}
					}
				}
			}
		}
	}
	None
}
pub fn ConvertToString(val: Option<Box<dyn std::any::Any>>) -> Option<String>
{
	if let Some(dval) = val
	{
		match dval.downcast::<String>()
		{
			Ok(x) => {
				return Some(*x);
			},
			Err(dval) => {
				match dval.downcast::<f64>()
				{
					Ok(x) => {
						return Some(x.to_string());
					},
					Err(notdouble) => {
						match notdouble.downcast::<i64>()
						{
							Ok(x) => {
								return Some(x.to_string());
							},
							Err(notbool) => {
								match notbool.downcast::<bool>()
								{
									Ok(x) => {
										return Some(x.to_string());
									},
									Err(_e) => {
										
									}
								}
							}
						}
					}
				}
			}
		}
	}
	None
}
pub fn ConvertToInt32(val: Option<Box<dyn std::any::Any>>) -> Option<i64>
{
	if let Some(dval) = val
	{
		match dval.downcast::<i64>()
		{
			Ok(x) => {
				return Some(*x);
			},
			Err(dval) => {
				match dval.downcast::<f64>()
				{
					Ok(x) => {
						return Some(*x as i64);
					},
					Err(notdouble) => {
						match notdouble.downcast::<bool>()
						{
							Ok(x) => {
								return Some(if *x {1} else {0});
							},
							Err(notbool) => {
								match notbool.downcast::<String>()
								{
									Ok(x) => {
										match (*x).parse::<i64>()
										{
											Ok(v) => return Some(v),
											Err(_e) => {}
										}
									},
									Err(_e) => {
										
									}
								}
							}
						}
					}
				}
			}
		}
	}
	None
}

