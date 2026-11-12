pub(super) struct Reader<'a> {
    pub bytes: &'a [u8],
    pub pos: usize,
}

impl<'a> Reader<'a> {
    pub fn take(&mut self, length: usize) -> Result<&'a [u8], String> {
        let end = self.pos.checked_add(length).ok_or("Byte length overflow")?;
        let value = self.bytes.get(self.pos..end).ok_or_else(|| {
            format!(
                "Unexpected end of data at byte {} (need {})",
                self.pos, length
            )
        })?;
        self.pos = end;
        Ok(value)
    }

    pub fn u8(&mut self) -> Result<u8, String> {
        Ok(self.take(1)?[0])
    }
    pub fn u16(&mut self) -> Result<u16, String> {
        Ok(u16::from_le_bytes(self.take(2)?.try_into().unwrap()))
    }
    pub fn u32(&mut self) -> Result<u32, String> {
        Ok(u32::from_le_bytes(self.take(4)?.try_into().unwrap()))
    }
    pub fn u64(&mut self) -> Result<u64, String> {
        Ok(u64::from_le_bytes(self.take(8)?.try_into().unwrap()))
    }
    pub fn i32(&mut self) -> Result<i32, String> {
        Ok(self.u32()? as i32)
    }
    pub fn i64(&mut self) -> Result<i64, String> {
        Ok(self.u64()? as i64)
    }
    pub fn bool(&mut self) -> Result<bool, String> {
        match self.u8()? {
            0 => Ok(false),
            1 => Ok(true),
            value => Err(format!("Invalid boolean: {}", value)),
        }
    }
    pub fn length(&mut self) -> Result<usize, String> {
        usize::try_from(self.u64()?).map_err(|_| "Length exceeds address space".into())
    }
    pub fn nonnegative(&mut self) -> Result<usize, String> {
        usize::try_from(self.i64()?).map_err(|_| "Negative or oversized function metadata".into())
    }
    pub fn ids(&mut self) -> Result<Vec<u32>, String> {
        let count = self.u32()?;
        (0..count).map(|_| self.u32()).collect()
    }
}
