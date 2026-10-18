import React from 'react';
import {
  List,
  Datagrid,
  TextField,
  DateField,
  UrlField,
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

export const PublicationList = (props: object) => (
  <List {...props}>
    <Datagrid>
      <TextField source="id" />
      <TextField source="title" />
      <TextField source="slug" />
      <TextField source="source" />
      <DateField source="publishedDate" />
      <EditButton />
      <ShowButton />
    </Datagrid>
  </List>
);

export const PublicationShow = (props: object) => (
  <Show {...props}>
    <SimpleShowLayout>
      <TextField source="id" />
      <TextField source="title" />
      <TextField source="slug" />
      <TextField source="summary" />
      <TextField source="source" />
      <UrlField source="externalUrl" />
      <UrlField source="coverImageUrl" />
      <DateField source="publishedDate" />
    </SimpleShowLayout>
  </Show>
);

export const PublicationCreate = (props: object) => (
  <Create {...props}>
    <SimpleForm>
      <TextInput source="title" />
      <TextInput source="slug" />
      <TextInput multiline source="summary" />
      <TextInput source="source" />
      <TextInput source="externalUrl" type="url" />
      <TextInput source="coverImageUrl" type="url" />
      <DateInput source="publishedDate" />
    </SimpleForm>
  </Create>
);

export const PublicationEdit = (props: object) => (
  <Edit {...props}>
    <SimpleForm>
      <TextInput disabled source="id" />
      <TextInput source="title" />
      <TextInput source="slug" />
      <TextInput multiline source="summary" />
      <TextInput source="source" />
      <TextInput source="externalUrl" type="url" />
      <TextInput source="coverImageUrl" type="url" />
      <DateInput source="publishedDate" />
    </SimpleForm>
  </Edit>
);
