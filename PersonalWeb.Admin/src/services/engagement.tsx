import React from 'react';
import {
  List,
  Datagrid,
  TextField,
  DateField,
  Create,
  SimpleForm,
  TextInput,
  DateInput,
  Edit,
  Show,
  SimpleShowLayout,
  EditButton,
  ShowButton
} from 'react-admin';

export const EngagementList = (props: object) => (
  <List {...props}>
    <Datagrid>
      <TextField source="id" />
      <TextField source="eventName" />
      <TextField source="engagementType" />
      <DateField source="eventDate" />
      <TextField source="location" />
      <EditButton />
      <ShowButton />
    </Datagrid>
  </List>
);

export const EngagementShow = (props: object) => (
  <Show {...props}>
    <SimpleShowLayout>
      <TextField source="id" />
      <TextField source="eventName" />
      <TextField source="engagementType" />
      <TextField source="topicOrRole" />
      <DateField source="eventDate" />
      <TextField source="location" />
      <TextField source="externalUrl" />
    </SimpleShowLayout>
  </Show>
);

export const EngagementCreate = (props: object) => (
  <Create {...props}>
    <SimpleForm>
      <TextInput source="eventName" />
      <TextInput source="engagementType" />
      <TextInput source="topicOrRole" />
      <DateInput source="eventDate" />
      <TextInput source="location" />
      <TextInput source="externalUrl" type="url" />
    </SimpleForm>
  </Create>
);

export const EngagementEdit = (props: object) => (
  <Edit {...props}>
    <SimpleForm>
      <TextInput disabled source="id" />
      <TextInput source="eventName" />
      <TextInput source="engagementType" />
      <TextInput source="topicOrRole" />
      <DateInput source="eventDate" />
      <TextInput source="location" />
      <TextInput source="externalUrl" type="url" />
    </SimpleForm>
  </Edit>
);
