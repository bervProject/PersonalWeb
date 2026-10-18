import React from 'react';
import {
  List,
  Datagrid,
  TextField,
  UrlField,
  Create,
  SimpleForm,
  TextInput,
  Edit,
  Show,
  SimpleShowLayout,
  EditButton,
  ShowButton
} from 'react-admin';

export const ProfileList = (props: object) => (
  <List {...props}>
    <Datagrid>
      <TextField source="id" />
      <TextField source="fullName" />
      <TextField source="titles" />
      <EditButton />
      <ShowButton />
    </Datagrid>
  </List>
);

export const ProfileShow = (props: object) => (
  <Show {...props}>
    <SimpleShowLayout>
      <TextField source="id" />
      <TextField source="fullName" />
      <TextField source="titles" />
      <TextField source="bio" />
      <UrlField source="resumeUrl" />
    </SimpleShowLayout>
  </Show>
);

export const ProfileCreate = (props: object) => (
  <Create {...props}>
    <SimpleForm>
      <TextInput source="fullName" />
      <TextInput source="titles" helperText='JSON array e.g. ["Software Engineer","AWS Community Builder"]' />
      <TextInput multiline source="bio" />
      <TextInput source="resumeUrl" type="url" />
    </SimpleForm>
  </Create>
);

export const ProfileEdit = (props: object) => (
  <Edit {...props}>
    <SimpleForm>
      <TextInput disabled source="id" />
      <TextInput source="fullName" />
      <TextInput source="titles" helperText='JSON array e.g. ["Software Engineer","AWS Community Builder"]' />
      <TextInput multiline source="bio" />
      <TextInput source="resumeUrl" type="url" />
    </SimpleForm>
  </Edit>
);
