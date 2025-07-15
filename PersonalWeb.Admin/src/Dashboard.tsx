import React, { useEffect, useState } from 'react';
import { Card, CardHeader, CardContent, Grid } from '@mui/material';
import { Title } from 'react-admin';

export default () => {
  const [count, setCount] = useState({
    totalAdmin: 0,
    totalUser: 0,
    totalExp: 0,
    totalEdu: 0,
    totalPorto: 0,
    totalPost: 0,
    totalDraft: 0
  });

  return (
    <Grid container spacing={2}>
      <Title title="Welcome to the administration" />
      <Grid size={6}>
        <Card>
          <CardHeader title="Users Data" />
          <CardContent>
            <p>Total Admin: {count.totalAdmin}</p>
            <p>Total Normal User: {count.totalUser}</p>
            <p>Total Experiences Data: {count.totalExp}</p>
            <p>Total Education Data: {count.totalEdu}</p>
            <p>Total Portofolio Data: {count.totalPorto}</p>
          </CardContent>
        </Card>
      </Grid>
      <Grid size={6}>
        <Card>
          <CardHeader title="Blog Data" />
          <CardContent>
            <p>Total Post: {count.totalPost}</p>
            <p>Total Draft: {count.totalDraft}</p>
          </CardContent>
        </Card>
      </Grid>
    </Grid>
  )
};
